using Flecs.NET.Core;

namespace Systems.Core.Fsm;

public partial class GameState
{
    public static void Setup(World world, Entity inputEntity)
    {
        world.System<Components.Core.Unique.GameState, Components.Core.Unique.MouseMode>()
        .Kind(Ecs.OnUpdate)
        .MultiThreaded()
        .Iter((Iter it, Field<Components.Core.Unique.GameState> gs, Field<Components.Core.Unique.MouseMode> mm) =>
        {
            var inputState = inputEntity.Get<Components.Input.InputState>();
            for (int i = 0; i < it.Count(); i++)
            {
                var gameState = gs[i];
                ref var mouseMode = ref mm[i];

                if (inputState.LeftPressed)
                {
                    gameState.CurrentGameState = Components.Core.Unique.GameStateEnum.Gameplay;
                    mouseMode.CurrentMouseMode = Components.Core.Unique.MouseModeEnum.Captured;
                }

                if (inputState.EscapePressed)
                {
                    gameState.CurrentGameState = Components.Core.Unique.GameStateEnum.Debug;
                    mouseMode.CurrentMouseMode = Components.Core.Unique.MouseModeEnum.Visible;
                }
            }
        });
    }
}
