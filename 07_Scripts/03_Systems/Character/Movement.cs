using Flecs.NET.Core;

namespace Systems.Character
{
    public static class Movement
    {
        public static void Setup(World world, Entity inputEntity)
        {
            world.System<Components.Character.Player, Components.Physics.Velocity, Components.Character.MovementStats, Components.Character.Character>()
                .Kind(Ecs.OnUpdate)
                .MultiThreaded()
                .Iter((Iter it, Field<Components.Character.Player> player, Field<Components.Physics.Velocity> v, Field<Components.Character.MovementStats> s, Field<Components.Character.Character> c) =>
                {
                    var inputState = inputEntity.Get<Components.Input.InputState>();

                    for (int i = 0; i < it.Count(); i++)
                    {
                        ref var velocity = ref v[i];
                        ref var character = ref c[i];
                        var stats = s[i];

                        if (character.IsGrounded)
                        {
                            var inputDir = new Components.Math.Vec2(
                                inputState.MoveRight ? 1 : inputState.MoveLeft ? -1 : 0,
                                inputState.MoveBackward ? 1 : inputState.MoveForward ? -1 : 0
                            ).Normalized();

                            var moveDir = new Components.Math.Vec3(inputDir.X, 0, inputDir.Y);

                            velocity.Value.X = moveDir.X * stats.Speed;
                            velocity.Value.Z = moveDir.Z * stats.Speed;
                        }
                    }
                });
        }
    }
}