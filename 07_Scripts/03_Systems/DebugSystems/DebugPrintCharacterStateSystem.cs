using Flecs.NET.Core;
using Components.Character;
using Kernel;

namespace Systems.Debug;

public static class DebugPrintCharacterStateSystem
{
    public static void Setup(World world)
    {
        world.System<CharacterStateComponent, CharacterMovementStatsComponent>()
            .Kind(Ecs.OnUpdate)
            .MultiThreaded()
            .Iter((Iter it, Field<CharacterStateComponent> cs, Field<CharacterMovementStatsComponent> ms) =>
            {
                for (int i = 0; i < it.Count(); i++)
                {
                    var characterState = cs[i];
                    var movementStats = ms[i];
                    // Log.Info(movementStats.CurrentSpeed.ToString());

                    Log.Info(characterState.CurrentState.ToString());
                }

            });
    }
    
}