using Flecs.NET.Core;
using Components.Singleton;

namespace Systems.Input;

public static class InputEmptyClickSystem
{
    public static void Setup(World world, Entity inputEntity)
    {
        world.System<SingletonGameStateComponent>()
            .Kind(Ecs.OnUpdate)
            .MultiThreaded()
            .Iter((Iter it, Field<SingletonGameStateComponent> gs) =>
            {
                for (int i = 0; i < it.Count(); i++)
                {
                    var gameState = gs[i];

                    if (gameState.CurrentGameState == GameStateEnum.Gameplay)
                    {
                        
                    }
                }

            });
    }
}