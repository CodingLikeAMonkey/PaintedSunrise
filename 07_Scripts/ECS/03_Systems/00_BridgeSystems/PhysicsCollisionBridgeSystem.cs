using Flecs.NET.Core;
using Godot;
using Components.Core;
using Components.Physics;
using Components.Character;
using Components.Math;

namespace Systems.Bridge;

public static class PhysicsCollisionBridgeSystem
{
    public static void Setup(World world)
{
    world.System<Entity, TransformComponent, PhysicsVelocityComponent>()
        .Kind(Ecs.OnUpdate)
        .Each((ref Entity entity, ref TransformComponent transform, ref PhysicsVelocityComponent velocity) =>
        {
            if (!Kernel.NodeRef<Node3D>.TryGet(entity, out Node3D node)) return;
            
            if (node is CharacterBody3D characterBody)
            {
                // Sync ECS → Godot
                characterBody.Velocity = velocity.Value.ToGodot();
                characterBody.GlobalPosition = transform.Position.ToGodot();

                // Run physics
                characterBody.MoveAndSlide();

                // Update grounded state
                if (entity.Has<CharacterComponent>())
                {
                    ref var character = ref entity.GetMut<CharacterComponent>();
                    character.IsGrounded = characterBody.IsOnFloor();
                }

                // Sync back to ECS
                transform.Position = (Vec3Component)characterBody.GlobalPosition;
                velocity.Value = (Vec3Component)characterBody.Velocity;
            }
        });
}
}