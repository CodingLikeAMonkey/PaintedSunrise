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


                        foreach (Node child in body.GetChildren())
                        {
                            if (child is Node3D node3D && node3D.IsInGroup("visual_body"))
                            {
                                Node3D visual = node3D;
                                // Apply yaw rotation (around Y-axis)
                                visual.Rotation = new Vector3(0, character.GhostBodyYaw, 0);
                                GD.Print($"Visual local Y: {visual.Rotation.Y}, global Y: {visual.GlobalRotation.Y}, ghost Y: {character.GhostBodyYaw}");
                                break;
                            }
                        }

                    }
                });
        }
    }
}