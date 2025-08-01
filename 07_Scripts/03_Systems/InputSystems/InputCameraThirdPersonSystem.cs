// Systems/Input/InputCameraThirdPersonSystem.cs
using Flecs.NET.Core;
using Components.Camera;
using Components.Input;
using Components.Math;

namespace Systems.Input
{
    public static class InputCameraThirdPersonSystem
    {
        public static void Setup(World world, Entity inputEntity)
        {
            world.System<CameraThirdPersonStateComponent, CameraThirdPersonConfigComponent>()
                .Kind(Ecs.OnUpdate)
                .Each((ref CameraThirdPersonStateComponent camState, ref CameraThirdPersonConfigComponent config) =>
                {
                    var inputState = inputEntity.Get<InputStateComponent>();

                    // --- Mouse Input ---
                    if (Godot.Input.MouseMode == Godot.Input.MouseModeEnum.Captured)
                    {
                        camState.rotationDegrees.Y -= inputState.MouseDelta.X * config.HorizontalMouseSensitivity;
                        camState.rotationDegrees.X -= inputState.MouseDelta.Y * config.VerticalMouseSensitivity;
                    }

                    // --- Controller Input ---
                    camState.lookvector = new Vec2Component(
                        inputState.RightStickInputDir.X,
                        inputState.RightStickInputDir.Y
                    );

                    camState.rotationDegrees.Y -= camState.lookvector.X * config.HorizontalControllerSensitivity;
                    camState.rotationDegrees.X -= (camState.lookvector.Y * config.VerticalControllerSensitivity)
                                                  * config.InvertVerticalControllerRotation;

                    // Clamp Pitch (X)
                    camState.rotationDegrees.X = MathUtilComponent.Clamp(
                        camState.rotationDegrees.X,
                        config.MaxPitch,
                        config.MinPitch
                    );
                });
        }
    }
}
