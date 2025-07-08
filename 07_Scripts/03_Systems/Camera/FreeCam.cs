using Flecs.NET.Core;
using Godot;
using Components.Core;
using Components.Camera;
using Components.Math;

namespace Systems.Camera
{
    public static class FreeCamSystem
    {
        public static void Setup(World world)
        {
            world.System<Transform, FreeCam>()
                .Kind(Ecs.OnUpdate)
                .Each((ref Transform t, ref FreeCam free) =>
                {
                    float delta = world.Entity("DeltaTime").Get<Kernel.DeltaTime>().Value;

                    if (Godot.Input.MouseMode == Godot.Input.MouseModeEnum.Captured)
                    {
                        t.Rotation.Y -= Kernel.InputHandler.MouseDelta.X / 1000f * free.Sensitivity;
                        t.Rotation.X -= Kernel.InputHandler.MouseDelta.Y / 1000f * free.Sensitivity;

                        t.Rotation.X = Mathf.Clamp(t.Rotation.X, -Mathf.Pi / 2, Mathf.Pi / 2);
                    }

                    if (Kernel.InputHandler.MouseWheel != 0)
                    {
                        free.CurrentVelocity = Mathf.Clamp(
                            free.CurrentVelocity * Mathf.Pow(free.SpeedScale, Kernel.InputHandler.MouseWheel),
                            free.MinSpeed,
                            free.MaxSpeed);
                    }

                    if (free.MovementDirection != new Vec3(0, 0, 0))
                    {
                        Quaternion quat = Quaternion.FromEuler((Vector3)t.Rotation);
                        Basis basis = new Basis(quat);

                        float speed = free.CurrentVelocity * (free.IsBoosted ? free.BoostMultiplier : 1f);
                        Vector3 moveDir = (Vector3)free.MovementDirection;

                        Vector3 deltaMove = basis * moveDir * speed * delta;

                        t.Position += (Vec3)deltaMove;
                    }
                });
        }
    }
}
