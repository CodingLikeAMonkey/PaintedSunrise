using Components.Core.Unique;
using Flecs.NET.Core;
using Kernel;

namespace Systems.Character
{
    public static class Movement
    {
        public static void Setup(World world, Entity inputEntity)
        {
            world.System<Components.Character.Player, Components.Physics.Velocity, Components.Character.MovementStats, Components.Character.Character>()
                .Kind(Ecs.OnUpdate)
                .MultiThreaded()
                .Iter((Iter it, Field<Components.Character.Player> p, Field<Components.Physics.Velocity> v, Field<Components.Character.MovementStats> s, Field<Components.Character.Character> c) =>
                {
                    var inputState = inputEntity.Get<Components.Input.InputState>();
                    float delta = world.Entity("DeltaTime").Get<Components.Core.Unique.DeltaTime>().Value;

                    for (int i = 0; i < it.Count(); i++)
                    {
                        ref var velocity = ref v[i];
                        ref var character = ref c[i];
                        var stats = s[i];
                        ref var player = ref p[i];

                        if (character.IsGrounded)
                        {
                            // last input detection
                            player.HasInput = inputState.LeftStickInputDir.Length() > 0.1f;

                            if (player.HasInput)
                            {
                                player.WalkInputHoldTime += delta;
                                player.LastInputDirection = inputState.LeftStickInputDir;
                                player.WasRotatingFromTap = true;
                                Log.Info(player.LastInputDirection.ToString());
                            }
                            else
                            {
                                player.WalkInputHoldTime = 0.0f;
                            }

                            // cam direciton
                            Components.Math.Vec3 inputVector = new Components.Math.Vec3(player.LastInputDirection.X, 0, player.LastInputDirection.Y);
                            // Vector3 direction = (Transform.Basis * inputVec).Normalized();
                            // Components.Math.Vec3 worldDirection = cameraRotation.Rotate(inputVector).Normalized();


                            // var inputDir = new Components.Math.Vec2(
                            //     inputState.MoveRight ? 1 : inputState.MoveLeft ? -1 : 0,
                            //     inputState.MoveBackward ? 1 : inputState.MoveForward ? -1 : 0
                            // ).Normalized();

                            // var moveDir = new Components.Math.Vec3(inputDir.X, 0, inputDir.Y);

                            // velocity.Value.X = moveDir.X * stats.Speed;
                            // velocity.Value.Z = moveDir.Z * stats.Speed;
                        }
                    }
                });
        }
    }
}