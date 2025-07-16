using Flecs.NET.Core;
using Godot;

namespace Systems.Physics;

public static class Collision
{
    public static void Setup(World world)
{
    world.System<Entity, Components.Core.Transform, Components.Physics.Velocity, Components.Physics.Collider>()
        .Kind(Ecs.OnUpdate)
        .Each((ref Entity entity, ref Components.Core.Transform transform, ref Components.Physics.Velocity velocity, ref Components.Physics.Collider collider) =>
        {
            if (!Kernel.NodeRef.TryGet(entity, out Node3D node)) return;
            
            if (node is CharacterBody3D characterBody)
            {
                // Sync ECS → Godot
                characterBody.Velocity = velocity.Value.ToGodot();
                characterBody.GlobalPosition = transform.Position.ToGodot();

                // Run physics
                characterBody.MoveAndSlide();

                // Update grounded state
                if (entity.Has<Components.Character.Character>())
                {
                    ref var character = ref entity.GetMut<Components.Character.Character>();
                    character.IsGrounded = characterBody.IsOnFloor();
                }

                // Sync back to ECS
                transform.Position = (Components.Math.Vec3)characterBody.GlobalPosition;
                velocity.Value = (Components.Math.Vec3)characterBody.Velocity;
            }
        });
}
}