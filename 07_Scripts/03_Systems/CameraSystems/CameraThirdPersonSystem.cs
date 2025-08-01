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
                .Each((ref TransformComponent transform, ref CameraThirdPersonStateComponent camState) =>
                {
                    transform.Rotation.X = camState.rotationDegrees.X;
                    transform.Rotation.Y = camState.rotationDegrees.Y;
                    transform.Rotation.Z = 0f;
                });
        }
    }
}
