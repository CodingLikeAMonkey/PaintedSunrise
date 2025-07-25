using Flecs.NET.Core;
using Components.Math;
using Components.Core;
using Components.Camera;
using Components.Input;

namespace Systems.Camera;
public static class CameraThirdPersonSystem
{
    public static void Setup(World world, Entity inputEntity)
    {
        world.System<TransformComponent, CameraThirdPersonConfigComponent, CameraThirdPersonStateComponent>()
        .Kind(Ecs.OnUpdate)
        .Iter((Iter it, Field<TransformComponent> t, Field<CameraThirdPersonConfigComponent> c, Field<CameraThirdPersonStateComponent> s) =>
        {
            var inputState = inputEntity.Get<InputStateComponent>();
            for (int i = 0; i < it.Count(); i++)
            {
                ref var transform = ref t[i];
                ref var config = ref c[i];
                ref var camState = ref s[i];

                if (Godot.Input.MouseMode == Godot.Input.MouseModeEnum.Captured)
                {
                    camState.rotationDegrees.Y -= inputState.MouseDelta.X * config.HorizontalMouseSensitivity;
                    camState.rotationDegrees.X -= inputState.MouseDelta.Y * config.VerticalMouseSensitivity;
                    camState.rotationDegrees.X = MathUtilComponent.Clamp(camState.rotationDegrees.X, config.MaxPitch, config.MinPitch);
                }

                camState.lookvector = new Vec2Component(
                    inputState.RightStickInputDir.X,
                    inputState.RightStickInputDir.Y
                );

                camState.rotationDegrees.Y -= camState.lookvector.X * config.HorizontalControllerSensitivity;
                camState.rotationDegrees.X -= (camState.lookvector.Y * config.VerticalControllerSensitivity) * config.InvertVerticalControllerRotation;
                camState.rotationDegrees.X = MathUtilComponent.Clamp(camState.rotationDegrees.X, config.MaxPitch, config.MinPitch);

                transform.Rotation.X = camState.rotationDegrees.X;
                transform.Rotation.Y = camState.rotationDegrees.Y;
                transform.Rotation.Z = 0f;
            }
        });
    }
}