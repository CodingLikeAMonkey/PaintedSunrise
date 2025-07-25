// Systems/Camera/FreeCamSystem.cs
using Flecs.NET.Core;
using System;
using Components.Math;

namespace Systems.Camera
{
    public static class FreeCamSystem
    {
        public static void Setup(World world, Entity inputEntity)
        {
            world.System<Components.Core.Transform, Components.Camera.FreeCam>()
                .Kind(Ecs.OnUpdate)
                .MultiThreaded()
                .Iter((Iter it, Field<Components.Core.Transform> t, Field<Components.Camera.FreeCam> free) =>
                {
                    var inputState = inputEntity.Get<Components.Input.InputState>();
                    float delta = world.Entity("DeltaTime").Get<Components.Core.Unique.DeltaTime>().Value;

                    bool mouseCaptured = Godot.Input.MouseMode == Godot.Input.MouseModeEnum.Captured;
                    var mouseDelta = inputState.MouseDelta;
                    int mouseWheel = inputState.MouseWheel;

                    for (int i = 0; i < it.Count(); i++)
                    {
                        ref var transform = ref t[i];
                        ref var freeCam = ref free[i];

                        if (mouseCaptured)
                        {
                            float pitch = -mouseDelta.Y / 1000f * freeCam.Sensitivity;
                            float yaw = -mouseDelta.X / 1000f * freeCam.Sensitivity;

                            Quaternion deltaPitch = Quaternion.FromAxisAngle(Vec3.Right, pitch);
                            Quaternion deltaYaw = Quaternion.FromAxisAngle(Vec3.Up, yaw);

                            transform.Rotation = deltaYaw * transform.Rotation;   // Yaw applied first (world space)
                            transform.Rotation = transform.Rotation * deltaPitch;  // Pitch applied after (local space)

                            // Optional: Normalize to avoid floating point drift
                            transform.Rotation = transform.Rotation.Normalized();
                        }


                        if (mouseWheel != 0)
                        {
                            freeCam.CurrentVelocity = Clamp(
                                freeCam.CurrentVelocity * MathF.Pow(freeCam.SpeedScale, mouseWheel),
                                freeCam.MinSpeed,
                                freeCam.MaxSpeed
                            );
                        }

                        if (freeCam.MovementDirection != Vec3.Zero)
                        {
                            Quaternion q = transform.Rotation;
                            Vec3 forward = q.Rotate(Vec3.Forward);
                            Vec3 right = q.Rotate(Vec3.Right);
                            Vec3 up = q.Rotate(Vec3.Up);

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
