using Flecs.NET.Core;
using Components.GDAP;
using Components.State;
using Kernel;

namespace Systems.Debug;

public static class DebugActiveActionSystem
{
    public static void Setup(World world)
    {
        world.System<StateCharacterIdle>()
            .Kind(Ecs.OnUpdate)
            .MultiThreaded()
            .Iter((Iter it, Field<StateCharacterIdle> sI) =>
            {
                for (int i = 0; i < it.Count(); i++)
                {
                    Entity entity = it.Entity(i);
                    var actionIdle = sI[i];
                }

            });
    }
}