using Flecs.NET.Core;
using Components.Math;

namespace Systems.Camera;
public static partial class ThirdPerson
{
    public static void Setup(World world, Entity inputEntity)
    {
        world.System<Components.Core.Transform, Components.Camera.ThirdPerson>()
        .Kind(Ecs.OnUpdate)
        .Iter((Iter it, Field<Components.Core.Transform> t, Field<Components.Camera.ThirdPerson> c) =>
        {
            var inputState = inputEntity.Get<Components.Input.InputState>();
            for (int i = 0; i < it.Count(); i++)
            {
                ref var transform = ref t[i];
                ref var camera = ref c[i];

                if (Godot.Input.MouseMode == Godot.Input.MouseModeEnum.Captured)
                {
                    camera.rotDeg.Y -= inputState.MouseDelta.X * camera.HorizontalMouseSensitivity;
                    camera.rotDeg.X -= inputState.MouseDelta.Y * camera.VerticalMouseSensitivity;
                    camera.rotDeg.X = MathUtil.Clamp(camera.rotDeg.X, camera.MaxPitch, camera.MinPitch);
                }

                camera.lookvector = new Components.Math.Vec2(
                    inputState.RightStickInputDir.X,
                    inputState.RightStickInputDir.Y
                );

                camera.rotDeg.Y -= camera.lookvector.X * camera.HorizontalControllerSensitivity;
                camera.rotDeg.X -= camera.lookvector.Y * camera.VerticalControllerSensitivity;
                camera.rotDeg.X = MathUtil.Clamp(camera.rotDeg.X, camera.MaxPitch, camera.MinPitch);

                transform.Rotation.X = camera.rotDeg.X;
                transform.Rotation.Y = camera.rotDeg.Y;
                transform.Rotation.Z = 0f;
            }
        });
    }
}
