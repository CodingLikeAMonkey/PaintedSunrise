// Systems/Input/FreeCamInputSystem.cs
using Flecs.NET.Core;
using Components.Camera;
using Components.Core.Unique;

namespace Systems.Input
{
    public static class FreeCamInputSystem
    {
        public static void Setup(World world)
        {
            world.System<FreeCam>()
                .Kind(Ecs.OnUpdate)
                .Each((ref FreeCam cam) =>
                {
                    var singleton = world.Lookup("Singleton");
                    if (!singleton.IsValid()) return;

                    var gameState = singleton.Get<GameState>();
                    if (gameState.CurrentGameState != GameStateEnum.Gameplay)
                        return;

                    // Build axis inputs
                    int x =  (Kernel.InputHandler.MoveRight ?  1 : 0)
                           - (Kernel.InputHandler.MoveLeft  ?  1 : 0);
                    int y =  (Kernel.InputHandler.MoveUp    ?  1 : 0)
                           - (Kernel.InputHandler.MoveDown  ?  1 : 0);
                    int z =  (Kernel.InputHandler.MoveForward ? 1 : 0)
                           - (Kernel.InputHandler.MoveBackward? 1 : 0);

                    cam.MovementDirection = new Components.Math.Vec3(x, y, z).Normalized();
                    cam.IsBoosted         = Kernel.InputHandler.Boost;
                });
        }
    }
}
