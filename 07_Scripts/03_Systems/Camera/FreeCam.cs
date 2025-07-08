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
                    float delta = world.Entity("DeltaTime").Get<Kernel.DeltaTime>().Value;

                    if (Godot.Input.MouseMode == Godot.Input.MouseModeEnum.Captured)
                    {
                        t.Rotation = new Vec3(
                            Clamp(t.Rotation.X - Kernel.InputHandler.MouseDelta.Y / 1000f * free.Sensitivity, -MathF.PI / 2, MathF.PI / 2),
                            t.Rotation.Y - Kernel.InputHandler.MouseDelta.X / 1000f * free.Sensitivity,
                            t.Rotation.Z
                        );
                    }

                    if (Kernel.InputHandler.MouseWheel != 0)
                    {
                        free.CurrentVelocity = Clamp(
                            free.CurrentVelocity * MathF.Pow(free.SpeedScale, Kernel.InputHandler.MouseWheel),
                            free.MinSpeed,
                            free.MaxSpeed
                        );
                    }

                    if (free.MovementDirection != Vec3.Zero)
                    {
                        Quaternion fullRotation = Quaternion.FromEuler(t.Rotation);
                        Vec3 fullForward = Vec3.Transform(new Vec3(0, 0, -1), fullRotation);

                        Vec3 flatForward = new Vec3(fullForward.X, 0, fullForward.Z).Normalized();
                        Vec3 flatRight = Vec3.Cross(Vec3.UnitY, flatForward).Normalized();

                        Vec3 inputDir = new Vec3(free.MovementDirection.X, 0, free.MovementDirection.Z);
                        Vec3 moveDir = flatForward * inputDir.Z + flatRight * inputDir.X;

                        float speed = free.CurrentVelocity * (free.IsBoosted ? free.BoostMultiplier : 1f);
                        Vec3 deltaMove = moveDir * speed * delta;

                        t.Position += deltaMove;
                    }
                });
        }

        private static float Clamp(float value, float min, float max)
        {
            return MathF.Max(min, MathF.Min(max, value));
        }
    }
}
