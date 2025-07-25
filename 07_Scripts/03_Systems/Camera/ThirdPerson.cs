using Flecs.NET.Core;
using Components.Math;

namespace Systems.Camera;
public static partial class ThirdPerson
{
    public static void Setup(World world, Entity inputEntity)
    {
        world.System<Components.Core.Transform, Components.Camera.ThirdPersonConfig, Components.Camera.ThirdPersonState>()
        .Kind(Ecs.OnUpdate)
        .Iter((Iter it, Field<Components.Core.Transform> t, Field<Components.Camera.ThirdPersonConfig> c, Field<Components.Camera.ThirdPersonState> s) =>
        {
            var inputState = inputEntity.Get<Components.Input.InputState>();
            for (int i = 0; i < it.Count(); i++)
            {
                ref var transform = ref t[i];
                ref var config = ref c[i];
                ref var camState = ref s[i];

                if (Godot.Input.MouseMode == Godot.Input.MouseModeEnum.Captured)
                {
                    camState.rotationDegrees.Y -= inputState.MouseDelta.X * config.HorizontalMouseSensitivity;
                    camState.rotationDegrees.X -= inputState.MouseDelta.Y * config.VerticalMouseSensitivity;
                    camState.rotationDegrees.X = MathUtil.Clamp(camState.rotationDegrees.X, config.MaxPitch, config.MinPitch);
                }

                camState.lookvector = new Components.Math.Vec2(
                    inputState.RightStickInputDir.X,
                    inputState.RightStickInputDir.Y
                );

                camState.rotationDegrees.Y -= camState.lookvector.X * config.HorizontalControllerSensitivity;
                camState.rotationDegrees.X -= (camState.lookvector.Y * config.VerticalControllerSensitivity) * config.InvertVerticalControllerRotation;
                camState.rotationDegrees.X = MathUtil.Clamp(camState.rotationDegrees.X, config.MaxPitch, config.MinPitch);

                transform.Rotation.X = camState.rotationDegrees.X;
                transform.Rotation.Y = camState.rotationDegrees.Y;
                transform.Rotation.Z = 0f;
            }
        });
    }
}