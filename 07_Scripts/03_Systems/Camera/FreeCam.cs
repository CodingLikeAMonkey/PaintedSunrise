// Systems/Camera/FreeCamSystem.cs
using Flecs.NET.Core;
using System;
using Components.Math;

namespace Systems.Camera
{
    public static class FreeCamSystem
    {
        public static void Setup(World world)
        {
            world.System<Components.Core.Transform, Components.Camera.FreeCam>()
                .Kind(Ecs.OnUpdate)
                .Each((ref Components.Core.Transform t, ref Components.Camera.FreeCam free) =>
                {
                    float delta = world.Entity("DeltaTime").Get<Components.Core.Unique.DeltaTime>().Value;

                    // Mouse look
                    if (Godot.Input.MouseMode == Godot.Input.MouseModeEnum.Captured)
                    {
                        t.Rotation = new Vec3(
                            Clamp(t.Rotation.X - Kernel.InputHandler.MouseDelta.Y / 1000f * free.Sensitivity, -MathF.PI / 2, MathF.PI / 2),
                            t.Rotation.Y - Kernel.InputHandler.MouseDelta.X / 1000f * free.Sensitivity,
                            t.Rotation.Z
                        );
                    }

                    // Zoom speed adjustments
                    if (Kernel.InputHandler.MouseWheel != 0)
                    {
                        free.CurrentVelocity = Clamp(
                            free.CurrentVelocity * MathF.Pow(free.SpeedScale, Kernel.InputHandler.MouseWheel),
                            free.MinSpeed,
                            free.MaxSpeed
                        );
                    }

                    // Movement
                    if (free.MovementDirection != Vec3.Zero)
                    {
                        // Build world-space basis
                        Quaternion q       = Quaternion.FromEuler(t.Rotation);
                        Vec3 forward        = Vec3.Transform(Vec3.Forward, q);
                        Vec3 right          = Vec3.Transform(Vec3.Right, q);
                        Vec3 up             = Vec3.Transform(Vec3.Up, q);

                        // Desired movement including vertical input
                        Vec3 md             = free.MovementDirection;
                        Vec3 desired        = forward * md.Z + right * md.X + up * md.Y;

                        float speed         = free.CurrentVelocity * (free.IsBoosted ? free.BoostMultiplier : 1f);
                        t.Position         += desired * speed * delta;
                    }
                });
        }

        private static float Clamp(float value, float min, float max)
        {
            return MathF.Max(min, MathF.Min(max, value));
        }
    }
}
