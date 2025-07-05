using Flecs.NET.Core;
using Godot;


namespace Systems.Camera;

public static class FreeCam
{
    public static void Setup(World world)
    {
        world.System<Components.Core.Transform, Components.Camera.Settings, Components.Camera.State, Components.Camera.FreeCam>()
            .Kind(Ecs.OnUpdate)
            .Each((ref Components.Core.Transform t, ref Components.Camera.Settings s, ref Components.Camera.State state, ref Components.Camera.FreeCam free) =>
            {
                // Get delta time
                float delta = world.Entity("DeltaTime").Get<Kernel.DeltaTime>().Value;

                // Handle rotation ONLY when mouse is captured
                if (Godot.Input.MouseMode == Godot.Input.MouseModeEnum.Captured)
                {
                    t.Rotation.Y -= Kernel.InputHandler.MouseDelta.X / 1000 * s.Sensitivity;
                    t.Rotation.X -= Kernel.InputHandler.MouseDelta.Y / 1000 * s.Sensitivity;
                    t.Rotation.X = Mathf.Clamp(t.Rotation.X, -Mathf.Pi / 2, Mathf.Pi / 2);
                }

                // Handle zoom regardless of capture state
                if (Kernel.InputHandler.MouseWheel != 0)
                {
                    state.CurrentVelocity = Mathf.Clamp(
                        state.CurrentVelocity * Mathf.Pow(s.SpeedScale, Kernel.InputHandler.MouseWheel),
                        s.MinSpeed,
                        s.MaxSpeed
                    );
                }

                // Movement - always enabled when keys are pressed
                if (state.MovementDirection != Vector3.Zero)
                {
                    // Convert Euler angles to Quaternion for basis creation
                    var quat = Quaternion.FromEuler(t.Rotation);
                    var basis = new Basis(quat);

                    var speed = state.CurrentVelocity * (state.IsBoosted ? s.BoostMultiplier : 1f);
                    t.Position += basis * state.MovementDirection * speed * delta;
                }
            });
    }
}
