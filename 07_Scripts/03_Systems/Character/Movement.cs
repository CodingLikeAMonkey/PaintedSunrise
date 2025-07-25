using Flecs.NET.Core;
using Components.Math;
using Kernel;
using System;
namespace Systems.Character
{
    public static class Movement
    {
        public static void Setup(World world, Entity inputEntity)
        {
            var camQuery = world.Query<Components.Camera.ThirdPersonConfig, Components.Core.Transform>();

            world.System<Components.Character.Player, Components.Physics.Velocity, Components.Character.MovementStats, Components.Character.Character>()
                .Kind(Ecs.OnUpdate)
                .MultiThreaded()
                .Iter((Iter it, Field<Components.Character.Player> player, Field<Components.Physics.Velocity> v, Field<Components.Character.MovementStats> s, Field<Components.Character.Character> c) =>
                {
                    var inputState = inputEntity.Get<Components.Input.InputState>();

                    camQuery.Each((Entity camEntity, ref Components.Camera.ThirdPersonConfig camCfg, ref Components.Core.Transform camTransform) =>
                    {
                        var forward = GetCameraForward(camTransform.Rotation);
                        var right = GetCameraRight(camTransform.Rotation);

                        // Flatten to XZ plane
                        forward.Y = 0;
                        right.Y = 0;
                        forward = Normalize(forward);
                        right = Normalize(right);

                        for (int i = 0; i < it.Count(); i++)
                        {
                            ref var velocity = ref v[i];
                            ref var character = ref c[i];
                            var stats = s[i];

                            if (character.IsGrounded)
                            {
                                // Get analog stick input
                                Vec2 inputDir = inputState.LeftStickInputDir;

                                if (inputDir.LengthSquared() > 0.001f) // Add deadzone threshold
                                {
                                    inputDir = inputDir.Normalized();

                                    // Convert 2D input to 3D movement direction
                                    Vec3 moveDir = Normalize(right * inputDir.X + forward * inputDir.Y); // Stick up = move away from camera

                                    velocity.Value.X = moveDir.X * stats.Speed;
                                    velocity.Value.Z = moveDir.Z * stats.Speed;
                                }
                                else
                                {
                                    velocity.Value.X = 0;
                                    velocity.Value.Z = 0;
                                }
                            }
                        }
                    });
                });
        }

        // --------- Math Functions ---------

        private static Vec3 GetCameraForward(Vec3 rotation)
        {
            float yaw = DegreesToRadians(rotation.Y);
            return new Vec3(
                (float)Math.Sin(yaw),
                0,
                (float)Math.Cos(yaw)
            );
        }

        private static Vec3 GetCameraRight(Vec3 rotation)
        {
            float yaw = DegreesToRadians(rotation.Y);
            return new Vec3(
                (float)Math.Cos(yaw),
                0,
                -(float)Math.Sin(yaw)
            );
        }

        private static Vec3 Normalize(Vec3 v)
        {
            float length = (float)System.Math.Sqrt(v.X * v.X + v.Y * v.Y + v.Z * v.Z);
            return length > 0 ? v / length : new Vec3(0, 0, 0);
        }

        private static float MoveToward(float current, float target, float maxDelta)
        {
            if (System.Math.Abs(target - current) <= maxDelta)
                return target;
            return current + System.Math.Sign(target - current) * maxDelta;
        }
        private static float DegreesToRadians(float degrees)
        {
            return degrees * ((float)Math.PI / 180f);
        }
    }
}
