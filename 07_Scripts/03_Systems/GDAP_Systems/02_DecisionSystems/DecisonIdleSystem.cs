using Flecs.NET.Core;
using Components.GDAP;
using Components.Character;
using System;
using Kernel;

namespace Systems.GDAP;

public static class DecisionIdleSystem
{
    public static void Setup(World world)
    {
        world.System<DecisionIdleComponent, ActionIdleComponent>()
            .Kind(Ecs.OnUpdate)
            .MultiThreaded()
            .Iter((Iter it, Field<DecisionIdleComponent> dI, Field<ActionIdleComponent> aI) =>
            {
                for (int i = 0; i < it.Count(); i++)
                {
                    ref var decisionIdle = ref dI[i];
                    decisionIdle.Desired = true;
                }

            });
    }
}