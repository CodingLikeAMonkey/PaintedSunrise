using Flecs.NET.Core;
using Godot;
using System;

namespace Systems.Core.Fsm;

public partial class GameState
{
    public static void Setup(World world)
    {
        world.System<Components.Core.Unique.GameState, Components.Core.Unique.MouseMode>()
        .Kind(Ecs.OnUpdate)
        .Each((ref Components.Core.Unique.GameState gameState, ref Components.Core.Unique.MouseMode mouseMode) =>
        {
            if (Kernel.InputHandler.LeftPressed)
            {
                gameState.CurrentGameState = Components.Core.Unique.GameStateEnum.Gameplay;
                mouseMode.CurrentMouseMode = Components.Core.Unique.MouseModeEnum.Captured;
            }

            if (Kernel.InputHandler.EscapePressed)
            {
                gameState.CurrentGameState = Components.Core.Unique.GameStateEnum.Debug;
                mouseMode.CurrentMouseMode = Components.Core.Unique.MouseModeEnum.Visible;
            }

        });
    }
}
