using Flecs.NET.Core;
using Godot;
using Components.Core;
using Components.Physics;
using Components.Math;
using Components.Character;

namespace Systems.Bridge
{
    public static class CharacterPhysicsBridgeSystem
    {
        public static void Setup(World world)
        {
            world.System<TransformComponent, PhysicsVelocityComponent, PhysicsGravityComponent>()
                .Kind(Ecs.OnUpdate)
                .Each((Entity entity, ref TransformComponent transform, ref PhysicsVelocityComponent velocity, ref PhysicsGravityComponent gravity) =>
                {
                    if (!Kernel.NodeRef.TryGet(entity, out Node3D node))
                    {
                        return;
                    }

                    if (node is not CharacterBody3D body)
                    {
                        return;
                    }

                    velocity.Value.Y += gravity.Acceleration * world.DeltaTime();

                    body.GlobalPosition = transform.Position.ToGodot();
                    body.Velocity = velocity.Value.ToGodot();
                    body.MoveAndSlide();

                    transform.Position = (Vec3Component)body.GlobalPosition;
                    velocity.Value = (Vec3Component)body.Velocity;

                    if (entity.Has<CharacterComponent>())
                    {
                        ref var character = ref entity.GetMut<CharacterComponent>();
                        character.IsGrounded = body.IsOnFloor();

                        Node3D visual;

                        // Access child node "%Player__Visual"
                        foreach (Node child in body.GetChildren())
                        {
                            if (child is Node3D node3D && node3D.IsInGroup("visual_body"))
                            {
                                visual = node3D;
                                // Apply yaw rotation (around Y-axis)
                                Vector3 rotation = visual.Rotation;
                                rotation.Y = character.GhostBodyYaw;
                                visual.Rotation = rotation;
                                break;
                            }
                        }

                    }
                });
        }
    }
}