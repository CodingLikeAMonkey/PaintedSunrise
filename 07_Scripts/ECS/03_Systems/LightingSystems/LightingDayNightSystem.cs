using Flecs.NET.Core;
using Components.Lighting;
using Components.Singleton;
using Components.Core;
using System;


namespace Ssytems.Lighting;

public static class LightingDayNightSystem
{
    public static void Setup(World world, Entity dayNightTime)
    {
        world.System<LightingSunComponent, LightingMoonComponent, LightingDirectionalFillComponent, TransformComponent>()
            .Kind(Ecs.OnUpdate)
            .MultiThreaded()
            .Iter((Iter it, Field<LightingSunComponent> s, Field<LightingMoonComponent> m, Field<LightingDirectionalFillComponent> d, Field<TransformComponent> t) => {
                SingletonDayTimeComponent daytimeComponent = dayNightTime.Get<SingletonDayTimeComponent>();
                for (int i = 0; i < it.Count(); i++)
                {
                    ref var transform = ref t[i];
                    float dayFraction = daytimeComponent.Hour / 24.0f;
                    transform.Rotation.X = dayFraction * MathF.Tau;
                }
            });
    }
}