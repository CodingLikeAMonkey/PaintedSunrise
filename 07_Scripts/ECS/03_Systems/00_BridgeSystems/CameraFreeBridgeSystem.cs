namespace Systems.Bridge;

using Godot;
using Flecs.NET.Core;
using Kernel.Math;
// using Components.Core;
using Components.Camera;
using Components.Math;

public static class CameraFreeBridgeSystem
{
    public static void Setup(World world)
    {
        // ECS → Godot sync
        world.System<Components.Core.TransformComponent, CameraFreeComponent>()
            .Kind(Ecs.PostUpdate)
            .Each((Entity entity, ref Components.Core.TransformComponent t, ref CameraFreeComponent free) =>
            {
                if (!Kernel.NodeRef<Camera3D>.TryGet(entity, out var node)) 
                    return;

                node.GlobalPosition = t.Position.ToGodot();
                node.GlobalRotation = t.Rotation.ToGodot();
            });

        // Godot input → ECS update
        world.System<CameraFreeComponent>()
            .Kind(Ecs.PostUpdate)
            .Each((ref CameraFreeComponent cam) =>
            {
                var direction = new Vec3Component(
                    Input.IsKeyPressed(Key.D) ? 1 : Input.IsKeyPressed(Key.A) ? -1 : 0,
                    Input.IsKeyPressed(Key.E) ? 1 : Input.IsKeyPressed(Key.Q) ? -1 : 0,
                    Input.IsKeyPressed(Key.S) ? 1 : Input.IsKeyPressed(Key.W) ? -1 : 0
                );

                cam.MovementDirection = direction.Normalize();
                cam.IsBoosted = Input.IsKeyPressed(Key.Shift);
            });
    }
}
