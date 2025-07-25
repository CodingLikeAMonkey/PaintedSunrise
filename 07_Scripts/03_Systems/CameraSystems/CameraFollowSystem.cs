using Flecs.NET.Core;
using Components.Core;
using Components.Camera;
using Components.Character;

namespace Systems.Camera;

public static class CameraFollowSystem
{
    public static void Setup(World world)
    {
        world.System<TransformComponent, CameraThirdPersonStateComponent>()
            .Kind(Ecs.OnUpdate)
            .MultiThreaded()
            .Iter((Iter it, Field<TransformComponent> t, Field<CameraThirdPersonStateComponent> c) =>
            {
                var playerQuery = world.Query<CharacterPlayerComponent, TransformComponent>();

                for (int i = 0; i < it.Count(); i++)
                {
                    ref var transform = ref t[i];
                }
            });
    }
}