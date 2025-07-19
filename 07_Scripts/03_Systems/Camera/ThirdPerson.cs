using Flecs.NET.Core;
using Components.Math;

namespace Systems.Camera;
public static partial class ThirdPerson
{
    public static void Setup(World world)
    {
        world.System<Components.Core.Transform, Components.Camera.ThirdPerson>()
        .Kind(Ecs.OnUpdate)
        .Iter((Iter it, Field<Components.Core.Transform> t, Field<Components.Camera.ThirdPerson> c) =>
        {
            for (int i = 0; i < it.Count(); i++)
            {
                ref var transform = ref t[i];
                ref var camera = ref c[i];

                // --- Mouse Rotation ---
                if (Godot.Input.MouseMode == Godot.Input.MouseModeEnum.Captured)
                {
                    // Yaw (horizontal rotation)
                    camera.rotDeg.Y -= Kernel.InputHandler.MouseDelta.X * camera.HorizontalMouseSensitivity;

                    // Pitch (vertical rotation)
                    camera.rotDeg.X -= Kernel.InputHandler.MouseDelta.Y * camera.VerticalMouseSensitivity;
                    camera.rotDeg.X = MathUtil.Clamp(camera.rotDeg.X, camera.MaxPitch, camera.MinPitch);
                }

                // --- Controller Rotation ---
                camera.lookvector = new Components.Math.Vec2(
                    Kernel.InputHandler.RightStickInputDir.X,
                    Kernel.InputHandler.RightStickInputDir.Y
                );

                camera.rotDeg.Y -= camera.lookvector.X * camera.HorizontalControllerSensitivity;
                camera.rotDeg.X -= camera.lookvector.Y * camera.VerticalControllerSensitivity;
                camera.rotDeg.X = MathUtil.Clamp(camera.rotDeg.X, camera.MaxPitch, camera.MinPitch);

                // --- Apply to Transform ---
                transform.Rotation.X = camera.rotDeg.X;
                transform.Rotation.Y = camera.rotDeg.Y;
                transform.Rotation.Z = 0f; // Assuming no roll
            }
        });
    }
}
