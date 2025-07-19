using Flecs.NET.Core;

namespace Systems.Physics
{
    public static class Gravity
    {
        public static void Setup(World world)
        {
            world.System<Components.Core.Transform, Components.Physics.Velocity, Components.Physics.Gravity>()
                .Kind(Ecs.OnUpdate)
                .MultiThreaded()
                .Iter((Iter it, Field<Components.Core.Transform> transField, Field<Components.Physics.Velocity> veloField, Field<Components.Physics.Gravity> graviField) =>
                {
                    // Get delta time from singleton entity
                    float deltaTime = it.World().Entity("DeltaTime").Get<Components.Core.Unique.DeltaTime>().Value;

                    foreach (int i in it)
                    {
                        ref var transform = ref transField[i];
                        ref var velocity = ref veloField[i];
                        ref var gravity = ref graviField[i];

                        velocity.Value.Y += gravity.Acceleration * deltaTime;
                    }
                });
        }
    }
}
