using Flecs.NET.Core;
using System;
using Components.Math;
using Components.Core;
using Components.Camera;
using Components.Input;
using Components.Singleton;


namespace Systems.Camera
{
    public static class CameraFreeSystem
    {
        public static void Setup(World world, Entity inputEntity)
        {
            world.System<TransformComponent, CameraFreeComponent>()
                .Kind(Ecs.OnUpdate)
                .MultiThreaded()
                .Iter((Iter it, Field<TransformComponent> t, Field<CameraFreeComponent> free) =>
                {
                    var inputState = inputEntity.Get<InputStateComponent>();
                    float delta = world.Entity("DeltaTime").Get<SingletonDeltaTimeComponent>().Value;

                    bool mouseCaptured = Godot.Input.MouseMode == Godot.Input.MouseModeEnum.Captured;
                    var mouseDelta = inputState.MouseDelta;
                    int mouseWheel = inputState.MouseWheel;

                    for (int i = 0; i < it.Count(); i++)
                    {
                        ref var transform = ref t[i];
                        ref var freeCam = ref free[i];

                        if (mouseCaptured)
                        {
                            transform.Rotation = new Vec3Component(
                                Clamp(transform.Rotation.X - mouseDelta.Y / 1000f * freeCam.Sensitivity, -MathF.PI / 2, MathF.PI / 2),
                                transform.Rotation.Y - mouseDelta.X / 1000f * freeCam.Sensitivity,
                                transform.Rotation.Z
                            );
                        }

                        if (mouseWheel != 0)
                        {
                            freeCam.CurrentVelocity = Clamp(
                                freeCam.CurrentVelocity * MathF.Pow(freeCam.SpeedScale, mouseWheel),
                                freeCam.MinSpeed,
                                freeCam.MaxSpeed
                            );
                        }

                        if (freeCam.MovementDirection != Vec3Component.Zero)
                        {
                            QuaternionComponent q = QuaternionComponent.FromEuler(transform.Rotation);
                            Vec3Component forward = Vec3Component.Transform(Vec3Component.Forward, q);
                            Vec3Component right = Vec3Component.Transform(Vec3Component.Right, q);
                            Vec3Component up = Vec3Component.Transform(Vec3Component.Up, q);

                            Vec3Component md = freeCam.MovementDirection;
                            Vec3Component desired = forward * md.Z + right * md.X + up * md.Y;

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
