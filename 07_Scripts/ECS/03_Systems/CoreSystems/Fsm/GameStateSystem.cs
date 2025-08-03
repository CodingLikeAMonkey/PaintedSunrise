using Flecs.NET.Core;
using Components.Singleton;
using Components.Input;
using Components.UI;
using Kernel;

namespace Systems.Core.Fsm;

public partial class GameStateSystem
{
    public static void Setup(World world, Entity inputEntity)
    {
        world.System<SingletonGameStateComponent, SingletonMouseModeComponent>()
        .Kind(Ecs.OnUpdate)
        .MultiThreaded()
        .Iter((Iter it, Field<SingletonGameStateComponent> gs, Field<SingletonMouseModeComponent> mm) =>
        {
            var inputState = inputEntity.Get<InputStateComponent>();
            for (int i = 0; i < it.Count(); i++)
            {
                var gameState = gs[i];
                ref var mouseMode = ref mm[i];

                if (inputState.LeftReleased)
                {
                    bool anyHovered = false;
                    var query = world.Query<UIInteractiveComponent>();

                    query.Each((ref UIInteractiveComponent interactiveComponent) =>
                        {
                            if (interactiveComponent.Hover)
                                anyHovered = true;
                        });

                    if (!anyHovered)
                    {
                        gameState.CurrentGameState = GameStateEnum.Gameplay;
                        mouseMode.CurrentMouseMode = MouseModeEnum.Captured;
                    }
                }

                if (inputState.EscapePressed && gameState.CurrentGameState == GameStateEnum.Gameplay)
                {
                    gameState.CurrentGameState = GameStateEnum.Debug;
                    mouseMode.CurrentMouseMode = MouseModeEnum.Visible;
                }
            }
        });
    }
}
