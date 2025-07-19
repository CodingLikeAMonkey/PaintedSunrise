using Flecs.NET.Core;

namespace Systems.Character
{
    public static class Movement
    {
        public static void Setup(World world)
        {
            world.System<Components.Character.Player, Components.Physics.Velocity, Components.Character.MovementStats, Components.Character.Character>()
                .Kind(Ecs.OnUpdate)
                .Each((ref Components.Character.Player player, ref Components.Physics.Velocity velocity, ref Components.Character.MovementStats stats, ref Components.Character.Character character) =>
                {
                    if (character.IsGrounded == true)
                    {
                        // Get normalized input direction
                    var inputDir = new Components.Math.Vec2(
                        Kernel.InputHandler.MoveRight ? 1 : Kernel.InputHandler.MoveLeft ? -1 : 0,
                        Kernel.InputHandler.MoveBackward ? 1 : Kernel.InputHandler.MoveForward ? -1 : 0
                    ).Normalized();

                    // Convert to 3D movement (relative to camera)
                    var moveDir = new Components.Math.Vec3(inputDir.X, 0, inputDir.Y);

                    // Apply movement speed (preserve vertical velocity)
                    velocity.Value.X = moveDir.X * stats.Speed;
                    velocity.Value.Z = moveDir.Z * stats.Speed;
                    }
                    
                });
        }
    }
}