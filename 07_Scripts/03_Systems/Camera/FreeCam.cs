using Flecs.NET.Core;
using Godot;


namespace Systems.Camera;

public static class FreeCam
{
    public static void Setup(World world)
    {
        world.System<Components.Core.Transform, Components.Camera.FreeCam>()
            .Kind(Ecs.OnUpdate)
            .Each((ref Components.Core.Transform t, ref Components.Camera.FreeCam free) =>
            {
                // Get delta time
                float delta = world.Entity("DeltaTime").Get<Kernel.DeltaTime>().Value;

                // Handle rotation ONLY when mouse is captured
                if (Godot.Input.MouseMode == Godot.Input.MouseModeEnum.Captured)
                {
                    t.Rotation.Y -= Kernel.InputHandler.MouseDelta.X / 1000 * free.Sensitivity;
                    t.Rotation.X -= Kernel.InputHandler.MouseDelta.Y / 1000 * free.Sensitivity;
                    t.Rotation.X = Mathf.Clamp(t.Rotation.X, -Mathf.Pi / 2, Mathf.Pi / 2);
                }

                // Handle zoom regardless of capture state
                if (Kernel.InputHandler.MouseWheel != 0)
                {
                    free.CurrentVelocity = Mathf.Clamp(
                        free.CurrentVelocity * Mathf.Pow(free.SpeedScale, Kernel.InputHandler.MouseWheel),
                        free.MinSpeed,
                        free.MaxSpeed
                    );
                }

                // Movement - always enabled when keys are pressed
                if (free.MovementDirection != Vector3.Zero)
                {
                    // Convert Euler angles to Quaternion for basis creation
                    var quat = Quaternion.FromEuler(t.Rotation);
                    var basis = new Basis(quat);

                    var speed = free.CurrentVelocity * (free.IsBoosted ? free.BoostMultiplier : 1f);
                    t.Position += basis * free.MovementDirection * speed * delta;
                }
            });
    }
}
