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
                .MultiThreaded()
                .Iter((Iter it, Field<CameraThirdPersonStateComponent> ct, Field<CameraThirdPersonConfigComponent> conf) =>
                {
                    var inputState = inputEntity.Get<InputStateComponent>();

                    for (int i = 0; i < it.Count(); i++)
                    {
                        ref var camState = ref ct[i];
                        var config = conf[i];
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
                    }
                });
        }
    }
}
