// Systems/Input/FreeCamInputSystem.cs
using Flecs.NET.Core;
using Components.Camera;
using Components.Core.Unique;

namespace Systems.Input
{
    public static class FreeCamInputSystem
    {
        public static void Setup(World world, Entity inputEntity)
        {
            world.System<FreeCam>()
                .Kind(Ecs.OnUpdate)
                .Each((ref FreeCam cam) =>
                {
                    var inputState = inputEntity.Get<Components.Input.InputState>();
                    var singleton = world.Lookup("Singleton");
                    if (!singleton.IsValid()) return;

                    var gameState = singleton.Get<GameState>();
                    if (gameState.CurrentGameState != GameStateEnum.Gameplay)
                        return;

                    // Build axis inputs
                    int x =  (inputState.MoveRight ?  1 : 0)
                           - (inputState.MoveLeft  ?  1 : 0);
                    int y =  (inputState.MoveUp    ?  1 : 0)
                           - (inputState.MoveDown  ?  1 : 0);
                    int z =  (inputState.MoveForward ? 1 : 0)
                           - (inputState.MoveBackward? 1 : 0);

                    cam.MovementDirection = new Components.Math.Vec3(x, y, z).Normalized();
                    cam.IsBoosted         = inputState.Boost;
                });
        }
    }
}
