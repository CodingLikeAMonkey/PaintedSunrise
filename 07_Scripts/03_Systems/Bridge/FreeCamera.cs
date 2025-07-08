namespace Systems.Bridge;

using Godot;
using Flecs.NET.Core;
using Kernel.Math;

public static class FreeCamera
{
    public static void Setup(World world)
    {
        // ECS → Godot sync
        world.System<Components.Core.Transform, Components.Camera.FreeCam>()
            .Kind(Ecs.OnUpdate)
            .Each((Entity entity, ref Components.Core.Transform t, ref Components.Camera.FreeCam free) =>
            {
                if (!CameraNodeRef.TryGet(entity, out var node)) 
                    return;

                node.GlobalPosition = t.Position.ToGodot();
                node.GlobalRotation = t.Rotation.ToGodot();
            });

        // Godot input → ECS update
        world.System<Components.Camera.FreeCam>()
            .Kind(Ecs.OnUpdate)
            .Each((ref Components.Camera.FreeCam cam) =>
            {
                var direction = new Components.Math.Vec3(
                    Input.IsKeyPressed(Key.D) ? 1 : Input.IsKeyPressed(Key.A) ? -1 : 0,
                    Input.IsKeyPressed(Key.E) ? 1 : Input.IsKeyPressed(Key.Q) ? -1 : 0,
                    Input.IsKeyPressed(Key.S) ? 1 : Input.IsKeyPressed(Key.W) ? -1 : 0
                );

                cam.MovementDirection = direction.Normalize();
                cam.IsBoosted = Input.IsKeyPressed(Key.Shift);
            });
    }
}
