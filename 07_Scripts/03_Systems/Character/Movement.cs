using Flecs.NET.Core;

namespace Systems.Character
{
    public static class Movement
    {
        public static void Setup(World world)
        {
            world.System<Components.Character.Player, Components.Physics.Velocity, Components.Character.MovementStats, Components.Character.Character>()
                .Kind(Ecs.OnUpdate)
                .MultiThreaded()
                .Iter((Iter it, Field<Components.Character.Player> player, Field<Components.Physics.Velocity> v, Field<Components.Character.MovementStats> s, Field<Components.Character.Character> c) =>
                {

                    for (int i = 0; i < it.Count(); i++)
                    {
                        ref var velocity = ref v[i];
                        ref var character = ref c[i];
                        var stats = s[i];

                        if (character.IsGrounded == true)
                        {
                            var inputDir = new Components.Math.Vec2(
                                Kernel.InputHandler.MoveRight ? 1 : Kernel.InputHandler.MoveLeft ? -1 : 0,
                                Kernel.InputHandler.MoveBackward ? 1 : Kernel.InputHandler.MoveForward ? -1 : 0
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