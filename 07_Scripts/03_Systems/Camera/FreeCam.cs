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
                .MultiThreaded()
                .Iter((Iter it, Field<Components.Core.Transform> t, Field<Components.Camera.FreeCam> free) =>
                {
                    // Fetch delta and input ONCE per frame, not per entity
                    float delta = world.Entity("DeltaTime").Get<Components.Core.Unique.DeltaTime>().Value;

                    bool mouseCaptured = Godot.Input.MouseMode == Godot.Input.MouseModeEnum.Captured;
                    var mouseDelta = Kernel.InputHandler.MouseDelta;
                    int mouseWheel = Kernel.InputHandler.MouseWheel;

                    for (int i = 0; i < it.Count(); i++)
                    {
                        ref var transform = ref t[i];
                        ref var freeCam = ref free[i];

                        // Mouse look
                        if (mouseCaptured)
                        {
                            transform.Rotation = new Vec3(
                                Clamp(transform.Rotation.X - mouseDelta.Y / 1000f * freeCam.Sensitivity, -MathF.PI / 2, MathF.PI / 2),
                                transform.Rotation.Y - mouseDelta.X / 1000f * freeCam.Sensitivity,
                                transform.Rotation.Z
                            );
                        }

                        // Zoom speed adjustments
                        if (mouseWheel != 0)
                        {
                            freeCam.CurrentVelocity = Clamp(
                                freeCam.CurrentVelocity * MathF.Pow(freeCam.SpeedScale, mouseWheel),
                                freeCam.MinSpeed,
                                freeCam.MaxSpeed
                            );
                        }

                        // Movement
                        if (freeCam.MovementDirection != Vec3.Zero)
                        {
                            Quaternion q = Quaternion.FromEuler(transform.Rotation);
                            Vec3 forward = Vec3.Transform(Vec3.Forward, q);
                            Vec3 right = Vec3.Transform(Vec3.Right, q);
                            Vec3 up = Vec3.Transform(Vec3.Up, q);

                            Vec3 md = freeCam.MovementDirection;
                            Vec3 desired = forward * md.Z + right * md.X + up * md.Y;

                            float speed = freeCam.CurrentVelocity * (freeCam.IsBoosted ? freeCam.BoostMultiplier : 1f);
                            transform.Position += desired * speed * delta;
                        }
                    }
                });
        }

        private static float Clamp(float value, float min, float max)
        {
            return MathF.Max(min, MathF.Min(max, value));
        }
    }
}
