using Flecs.NET.Core;
using Components.Character;
using Components.Core;
using Kernel;
using Components.Singleton;
using Components.Time;

namespace Systems.Character;

public static class CharacterLastPositonSystem
{
    public static void Setup(World world)
    {
        world.System<CharacterLastPositionComponent, CharacterComponent, TransformComponent>()
            .Kind(Ecs.OnUpdate)
            .MultiThreaded()
            .Iter((Iter it, Field<CharacterLastPositionComponent> lp, Field<CharacterComponent> c, Field<TransformComponent> t) =>
            {
                float delta = world.Entity("DeltaTime").Get<SingletonDeltaTimeComponent>().Value;

                for (int i = 0; i < it.Count(); i++)
                {
                    Entity entity = it.Entity(i);
                    ref var lastPositon = ref lp[i];
                    var character = c[i];
                    var transform = t[i];


                    if (character.IsGrounded)
                    {
                        if (!entity.Has<TimerComponent>())
                        {
                            entity.Set(new TimerComponent(duration: 2.0f));
                            continue;
                        }
                        var timer = entity.Get<TimerComponent>();

                        if (timer.IsRunning)
                        {
                            timer.Elapsed += delta;

                            if (timer.Elapsed >= timer.Duration)
                            {
                                lastPositon.Value = transform.Position;
                                timer.Elapsed = 0;
                            }
                            entity.Set(timer);
                        }
                    }

                }
            });
    }
}