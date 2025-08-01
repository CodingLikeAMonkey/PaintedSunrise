using Flecs.NET.Core;
using Components.Math;
using Components.Core;
using Components.Camera;

namespace Systems.Camera
{
    public static class CameraThirdPersonSystem
    {
        public static void Setup(World world)
        {
            world.System<TransformComponent, CameraThirdPersonStateComponent>()
                .Kind(Ecs.OnUpdate)
                .MultiThreaded()
                .Iter((Iter it, Field<TransformComponent> t, Field<CameraThirdPersonStateComponent> c) =>
                {
                    for (int i = 0; i < it.Count(); i++)
                    {
                        ref var transform = ref t[i];
                        var camState = c[i];
                        transform.Rotation.X = camState.rotationDegrees.X;
                        transform.Rotation.Y = camState.rotationDegrees.Y;
                        transform.Rotation.Z = 0f;
                    }
                });
        }
    }
}
