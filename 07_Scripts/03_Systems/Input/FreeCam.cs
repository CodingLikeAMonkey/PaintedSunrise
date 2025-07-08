using Components.Core.Unique;
using Flecs.NET.Core;

namespace Systems.Input;

public static class FreeCam
{
    public static void Setup(World world)
    {
        world.System<Components.Camera.FreeCam>()
            .Kind(Ecs.OnUpdate)
            .Each((ref Components.Camera.FreeCam cam) =>
            {
                var singleton = world.Lookup("Singleton");
                if (singleton.IsValid())
                {
                    var gameState = singleton.Get<GameState>();
                    if (gameState.CurrentGameState == GameStateEnum.Gameplay)
                    {
                        int x = Kernel.InputHandler.MoveRight ? -1 : Kernel.InputHandler.MoveLeft ? 1 : 0;
                        int y = Kernel.InputHandler.MoveUp ? 1 : Kernel.InputHandler.MoveDown ? -1 : 0;
                        int z = Kernel.InputHandler.MoveBackward ? -1 : Kernel.InputHandler.MoveForward ? 1 : 0;

                        cam.MovementDirection = new Components.Math.Vec3(x, y, z).Normalized();
                        cam.IsBoosted = Kernel.InputHandler.Boost;
                    }
                }

            });
    }
}