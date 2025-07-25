using Flecs.NET.Core;
using Components.Core;
using Components.Physics;

namespace Systems.Physics;
public static class Movement
{
    public static void Setup(World world)
    {
        world.System<TransformComponent, PhysicsVelocityComponent>()
            .Kind(Ecs.OnUpdate)
            .Iter((Iter it, Field<TransformComponent> transField, Field<PhysicsVelocityComponent> veloField) =>
            {
                // float deltaTime = it.World().Entity("DeltaTime").Get<Components.Core.Unique.DeltaTime>().Value;

                // foreach (int i in it)
                // {
                //     ref var transform = ref transField[i];
                //     ref var velocity = ref veloField[i];
                //     transform.Position += velocity.Value * deltaTime;
                // hello world
                // }
            });
    }
}