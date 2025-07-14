using Flecs.NET.Core;
using Godot;

namespace Systems.Bridge; 
public static class TransformSync {
    public static void Setup(World world) {
        world.System<Components.Core.Transform>()
            .Kind(Ecs.PostUpdate)
            .Each((Entity entity, ref Components.Core.Transform transform) =>
            {
                if (Kernel.NodeRef.TryGet(entity, out Node3D node))
                {
                    node.GlobalPosition = (Godot.Vector3)transform.Position;
                    node.GlobalRotation = (Godot.Vector3)transform.Rotation;
                    node.Scale = (Godot.Vector3)transform.Scale;
                }
            });
        }
    }