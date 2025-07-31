using Flecs.NET.Core;
using Components.GDAP;
using Components.Character;
using Kernel;

namespace Systems.Debug;

public static class DebugActiveActionSystem
{
    public static void Setup(World world)
    {
        world.System<CharacterStateComponent>()
            .Kind(Ecs.OnUpdate)
            .MultiThreaded()
            .Iter((Iter it, Field<CharacterStateComponent> cs) =>
            {
                for (int i = 0; i < it.Count(); i++)
                {
                    Entity entity = it.Entity(i);
                    var actionIdle = cs[i];
                }

            });
    }
}