using System;
using Flecs.NET.Core;
using Components.Core;
using Components.Math;
using Kernel.Math;
using Kernel;
using System.ComponentModel.Design.Serialization;
using Components.Core.Unique;

namespace Systems.Character
{
    public static class Movement
    {
        public static void Setup(World world, Entity inputEntity)
        {
            world.System<Components.Character.Player, Components.Physics.Velocity, Components.Character.MovementStats, Components.Character.Character, Components.Character.State>()
                .Kind(Ecs.OnUpdate)
                .MultiThreaded()
                .Iter((Iter it, Field<Components.Character.Player> player, Field<Components.Physics.Velocity> velocity, Field<Components.Character.MovementStats> stats, Field<Components.Character.Character> character, Field<Components.Character.State> state) =>
                {
                    float delta = world.Entity("DeltaTime").Get<Components.Core.Unique.DeltaTime>().Value;
                    var input = inputEntity.Get<Components.Input.InputState>();

                    for (int i = 0; i < it.Count(); i++)
                    {
                        ref var vel = ref velocity[i];
                        ref var chara = ref character[i];
                        ref var st = ref state[i];
                        ref var plr = ref player[i];
                        var mvStats = stats[i];

                        MovementMotion(world, ref vel, ref chara, mvStats, input, ref plr, ref st, delta);


                    }
                });
        }

        private static Components.Physics.Velocity MovementMotion(
            World world,
            ref Components.Physics.Velocity velocity,
            ref Components.Character.Character character,
            Components.Character.MovementStats stats,
            Components.Input.InputState input,
            ref Components.Character.Player player,
            ref Components.Character.State state,
            double delta)
        {
            Vec2 inputDir = new Vec2(
                input.MoveRight ? 1 : input.MoveLeft ? -1 : 0,
                input.MoveBackward ? 1 : input.MoveForward ? -1 : 0
            ).Normalized();

            bool hasInput = inputDir.Length() > 0.1f;

            UpdateInputTracking(inputDir, delta, ref player);


            // Get camera-relative move direction
            Vec3 cameraDir = GetCameraRelativeDirection(world, ref player);

            // Rotation
            if (ShouldRotate(hasInput, character, player))
            {
                player.CurrentRotationY = ApplyRotation(
                    cameraDir,
                    player.CurrentRotationY,
                    delta,
                    hasInput,
                    stats.TurnSpeed,
                    out bool stopRotating
                );

                if (stopRotating)
                    player.WasRotatingFromTap = false;
            }

            // Movement logic
            if (character.IsGrounded)
            {
                velocity = ApplyMovement(ref velocity, ref state, inputDir, cameraDir, ref player, stats, delta, hasInput);
            }

            return velocity;
        }

        private static void UpdateInputTracking(Vec2 inputDir, double delta, ref Components.Character.Player player)
        {
            if (inputDir.Length() > 0.1f)
            {
                player.WalkInputHoldTime += (float)delta;
                player.LastInputDirection = inputDir;
                player.WasRotatingFromTap = true;
            }
            else
            {
                player.WalkInputHoldTime = 0f;
            }
        }

        private static Vec3 GetCameraRelativeDirection(World world, ref Components.Character.Player player)
        {
            Vec3 inputVec = new Vec3(player.LastInputDirection.X, 0, player.LastInputDirection.Y);
            Vec3 result = Vec3.Zero;

            var camQuery = world.Query<Components.Camera.ThirdPersonState>();
            camQuery.Each((Entity e, ref Components.Camera.ThirdPersonState cam) =>
            {
                var basis = Basis.FromQuaternion(cam.Rotation);
                result = (basis * inputVec).Normalized();
            });

            return result;
        }

        private static bool ShouldRotate(bool hasInput, Components.Character.Character character, Components.Character.Player player)
        {
            return character.IsGrounded && (hasInput || player.WasRotatingFromTap);
        }

        private static float WrapAngle(float angle)
        {
            angle = MathExtensions.PosMod(angle + (float)Math.PI, (float)Math.Tau);
            return angle - (float)Math.PI;
        }

        private static float ApplyRotation(
            Vec3 direction,
            float currentRotationY,
            double delta,
            bool hasInput,
            float turnSpeed,
            out bool stopRotating
        )
        {
            float targetYaw = MathExtensions.Atan2(direction.X, direction.Z);
            float newYaw = MathExtensions.LerpAngle(currentRotationY, targetYaw, turnSpeed * (float)delta);

            float angleDiff = MathF.Abs(WrapAngle(newYaw - targetYaw));
            stopRotating = !hasInput && angleDiff < 0.05f;

            return newYaw;
        }

        private static Components.Physics.Velocity ApplyMovement(
            ref Components.Physics.Velocity velocity,
            ref Components.Character.State state,
            Vec2 inputDir,
            Vec3 moveDir,
            ref Components.Character.Player player,
            Components.Character.MovementStats stats,
            double delta,
            bool hasInput)
        {
            if (hasInput && player.WalkInputHoldTime > stats.TapThreshold)
            {
                float speed = inputDir.Length() < stats.WalkThreshold ? stats.WalkSpeed : stats.Speed;
                state.CurrentState = inputDir.Length() < stats.WalkThreshold ? Components.Character.CharacterStateEnum.WalkState : Components.Character.CharacterStateEnum.RunState;

                var vel = velocity.Value;
                vel.X = Vec3.MoveTowardsF(vel.X, (moveDir * speed).X, stats.Acceleration * (float)delta);
                vel.Z = Vec3.MoveTowardsF(vel.Z, (moveDir * speed).Z, stats.Acceleration * (float)delta);
                velocity.Value = vel;
            }
            else
            {
                var vel = velocity.Value;
                vel.X = Vec3.MoveTowardsF(vel.X, 0, stats.Friction * (float)delta);
                vel.Z = Vec3.MoveTowardsF(vel.Z, 0, stats.Friction * (float)delta);
                velocity.Value = vel;

                if (!hasInput)
                    state.CurrentState = Components.Character.CharacterStateEnum.IdleState;
            }

            return velocity;
        }
    }
}
