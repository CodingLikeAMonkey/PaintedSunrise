using Flecs.NET.Core;

namespace Systems.Physics;
public static class Movement
{
    public static void Setup(World world)
    {
        world.System<Components.Core.Transform, Components.Physics.Velocity>()
            .Kind(Ecs.OnUpdate)
            .Iter((Iter it, Field<Components.Core.Transform> transField, Field<Components.Physics.Velocity> veloField) =>
            {
                // float deltaTime = it.World().Entity("DeltaTime").Get<Components.Core.Unique.DeltaTime>().Value;

                // foreach (int i in it)
                // {
                //     ref var transform = ref transField[i];
                //     ref var velocity = ref veloField[i];
                //     transform.Position += velocity.Value * deltaTime;
                // }
            });
    }
}