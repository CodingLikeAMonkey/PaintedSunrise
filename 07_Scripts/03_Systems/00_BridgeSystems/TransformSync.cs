using Flecs.NET.Core;
using Godot;
using Components.Core;
using Components.Physics;

namespace Systems.Bridge; 
public static class TransformSync {
    public static void Setup(World world) {
        world.System<TransformComponent>()
            .Kind(Ecs.PostUpdate)
            .Without<PhysicsColliderComponent>()
            .Each((Entity entity, ref TransformComponent transform) =>
            {
                // if (Kernel.NodeRef.TryGet(entity, out Node3D node))
                // {
                //     node.GlobalPosition = (Godot.Vector3)transform.Position;
                //     node.GlobalRotation = (Godot.Vector3)transform.Rotation;
                //     node.Scale = (Godot.Vector3)transform.Scale;
                // }
            });
        }
    }