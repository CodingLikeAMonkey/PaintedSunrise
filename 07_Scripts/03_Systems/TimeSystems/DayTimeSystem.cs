using Flecs.NET.Core;
using Components.Singleton;

namespace Systems.Time;

public static class DayTimeSystem
{
    public static void Setup(World world)
    {
        world.System<SingletonDayTimeComponent>()
            .Kind(Ecs.OnUpdate)
            .MultiThreaded()
            .Iter((Iter it, Field<SingletonDayTimeComponent> dt) =>
            {
                float delta = world.Entity("DeltaTime").Get<SingletonDeltaTimeComponent>().Value;
                for (int i = 0; i < it.Count(); i++)
                {
                    ref var dayTime = ref dt[i];

                    float secondsPerGameHour = dayTime.TimeScale * 60.0f;
                    float gameHoursPerSecond = 1f / secondsPerGameHour;

                    dayTime.Hour += delta * gameHoursPerSecond;

                    if (dayTime.Hour >= 24.0f)
                    {
                        dayTime.Hour -= 24.0f;
                        dayTime.Day++;
                    }
                }
            });
    }
}