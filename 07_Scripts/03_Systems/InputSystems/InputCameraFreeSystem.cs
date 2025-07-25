// Systems/Input/FreeCamInputSystem.cs
using Flecs.NET.Core;
using Components.Camera;
using Components.Singleton;
using Components.Input;
using Components.Math;

namespace Systems.Input
{
    public static class InputCameraFreeSystem
    {
        public static void Setup(World world, Entity inputEntity)
        {
            world.System<CameraFreeComponent>()
                .Kind(Ecs.OnUpdate)
                .Each((ref CameraFreeComponent cam) =>
                {
                    var inputState = inputEntity.Get<InputStateComponent>();
                    var singleton = world.Lookup("Singleton");
                    if (!singleton.IsValid()) return;

                    var gameState = singleton.Get<SingletonGameStateComponent>();
                    if (gameState.CurrentGameState != GameStateEnum.Gameplay)
                        return;

                    // Build axis inputs
                    int x =  (inputState.MoveRight ?  1 : 0)
                           - (inputState.MoveLeft  ?  1 : 0);
                    int y =  (inputState.MoveUp    ?  1 : 0)
                           - (inputState.MoveDown  ?  1 : 0);
                    int z =  (inputState.MoveForward ? 1 : 0)
                           - (inputState.MoveBackward? 1 : 0);

                    cam.MovementDirection = new Vec3Component(x, y, z).Normalized();
                    cam.IsBoosted         = inputState.Boost;
                });
        }
    }
}
