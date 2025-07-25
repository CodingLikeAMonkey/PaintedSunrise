using System.ComponentModel.Design.Serialization;
using Components.Core.Unique;
using Flecs.NET.Core;
using Kernel;

namespace Systems.Character
{
    public static class Movement
    {
        public static void Setup(World world, Entity inputEntity)
        {
            var camQuery = world.Query<Components.Camera.ThirdPersonConfig>();
            world.System<
            Components.Character.Player,
            Components.Physics.Velocity,
            Components.Character.MovementStats,
            Components.Character.Character,
            Components.Core.Transform>()
                .Kind(Ecs.OnUpdate)
                .MultiThreaded()
                .Iter((Iter it,
                Field<Components.Character.Player> p,
                Field<Components.Physics.Velocity> v,
                Field<Components.Character.MovementStats> s,
                Field<Components.Character.Character> c,
                Field<Components.Core.Transform> t) =>
                {
                    var inputState = inputEntity.Get<Components.Input.InputState>();
                    float delta = world.Entity("DeltaTime").Get<Components.Core.Unique.DeltaTime>().Value;
                    var cameraTransform = camQuery.First().Get<Components.Core.Transform>();

                    for (int i = 0; i < it.Count(); i++)
                    {
                        ref var velocity = ref v[i];
                        ref var character = ref c[i];
                        var stats = s[i];
                        ref var player = ref p[i];
                        ref var transform = ref t[i];


                        if (character.IsGrounded)
                        {
                            player.HasInput = inputState.LeftStickInputDir.Length() > 0.1f;

                            if (player.HasInput)
                            {
                                player.WalkInputHoldTime += delta;
                                player.LastInputDirection = inputState.LeftStickInputDir;
                                player.WasRotatingFromTap = true;
                            }
                            else
                            {
                                player.WalkInputHoldTime = 0.0f;
                            }

                            Components.Math.Vec3 inputVector = new Components.Math.Vec3(player.LastInputDirection.X, 0, player.LastInputDirection.Y);


                            // Convert camera basis to a forward-facing direction
                            var camBasis = cameraTransform.Rotation.ToBasis();
                            Components.Math.Vec3 forward = new Components.Math.Vec3(camBasis.Z.X, 0, camBasis.Z.Z).Normalized();
                            Components.Math.Vec3 right = new Components.Math.Vec3(camBasis.X.X, 0, camBasis.X.Z).Normalized();


                            Components.Math.Vec3 cameraDirection =
                                (forward * player.LastInputDirection.Y + right * player.LastInputDirection.X).Normalized();


                            if (character.IsGrounded)
                            {
                                if (player.HasInput && player.WalkInputHoldTime > stats.TapThreshold)
                                {
                                    float currentSpeed = (inputState.LeftStickInputDir.Length() < stats.WalkThreshold) ? stats.WalkSpeed : stats.Speed;

                                    velocity.Value.X = Components.Math.MathUtils.MoveToward(velocity.Value.X, cameraDirection.X * currentSpeed, stats.Acceleration * delta);
                                    velocity.Value.Z = Components.Math.MathUtils.MoveToward(velocity.Value.Z, cameraDirection.Z * currentSpeed, stats.Acceleration * delta);

                                }
                                else
                                {
                                    velocity.Value.X = Components.Math.MathUtils.MoveToward(velocity.Value.X, 0, stats.Friction * delta);
                                    velocity.Value.Z = Components.Math.MathUtils.MoveToward(velocity.Value.Z, 0, stats.Friction * delta);
                                }
                            }
                            Log.Info("Rotation: " + cameraTransform.Rotation.ToString()+ " Position: " + cameraTransform.Position.ToString() );
                        }
                    }
                });
        }
    }
}