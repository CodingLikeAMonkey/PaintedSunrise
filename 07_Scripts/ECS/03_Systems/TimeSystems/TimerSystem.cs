using Flecs.NET.Core;

using Components.Time;
using Components.Singleton;
using Godot;

public class TimerSystem
{
    public static void Setup(World world)
    {
        world.System<TimerComponent>()
        .Kind(Ecs.OnUpdate)
        .MultiThreaded()
        .Iter((Iter it, Field<TimerComponent> t) =>
        {
            float delta = world.Entity("DeltaTime").Get<SingletonDeltaTimeComponent>().Value;

            for (int i = 0; i < it.Count(); i++)
            {
                ref var timer = ref t[i];

                if (!timer.IsRunning)
                    return;

                timer.Elapsed += delta;

                if (timer.Elapsed >= timer.Duration)
                {
                    timer.Elapsed = timer.Duration;
                    timer.IsRunning = false;
                }
            }

        });
    }
}