using Flecs.NET.Core;
using Components.Core;
using Components.Physics;
using Components.Singleton;

namespace Systems.Physics
{
    public static class PhysicsGravitySystem
    {
        public static void Setup(World world)
        {
            world.System<TransformComponent, PhysicsVelocityComponent, PhysicsGravityComponent>()
                .Kind(Ecs.OnUpdate)
                .MultiThreaded()
                .Iter((Iter it, Field<TransformComponent> transField, Field<PhysicsVelocityComponent> veloField, Field<PhysicsGravityComponent> graviField) =>
                {
                    // Get delta time from singleton entity
                    float deltaTime = it.World().Entity("DeltaTime").Get<SingletonDeltaTimeComponent>().Value;

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
