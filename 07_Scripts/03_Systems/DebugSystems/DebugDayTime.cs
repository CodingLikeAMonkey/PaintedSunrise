using Flecs.NET.Core;
using Components.Singleton;
using Kernel;

namespace Systems.Debug;

public static class DebugDayTime
{
    public static void Setup(World world)
    {
        world.System<SingletonDayTimeComponent>()
            .Kind(Ecs.OnUpdate)
            .MultiThreaded()
            .Iter((Iter it, Field<SingletonDayTimeComponent> dt) =>
            {
                for (int i = 0; i < it.Count(); i++)
                {
                    var dayTime = dt[i];
                    Log.Info("Day: " + dayTime.Day + " Hour: " + dayTime.Hour + " Time progress: " + dayTime.TimeScale);
                }

            });
    }
}
