using Flecs.NET.Core;
using Components.Math;
using Components.Core;
using System.Runtime.ExceptionServices;

namespace Systems.Camera;

public static class CameraFollowSystem
{
    public static void Setup(World world)
    {
        world.System<Components.Core.Transform, Components.Camera.ThirdPersonState>()
            .Kind(Ecs.OnUpdate)
            .MultiThreaded()
            .Iter((Iter it, Field<Components.Core.Transform> t, Field<Components.Camera.ThirdPersonState> c) =>
            {
                var playerQuery = world.Query<Components.Character.Player, Components.Core.Transform>();

                for (int i = 0; i < it.Count(); i++)
                {
                    ref var transform = ref t[i];
                }
            });
    }
}