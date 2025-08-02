using Flecs.NET.Core;
using Components.Singleton;
using Kernel;

namespace Systems.Debug;

public static class DebugGameStateSystem
{
    public static void Setup(World world)
    {
        world.System<SingletonGameStateComponent>()
            .Kind(Ecs.OnUpdate)
            .MultiThreaded()
            .Iter((Iter it, Field<SingletonGameStateComponent> gs) =>
            {
                for (int i = 0; i < it.Count(); i++)
                {
                    var gameState = gs[i];
                    Log.Info(gameState.CurrentGameState.ToString());
                }
            });
    }
}