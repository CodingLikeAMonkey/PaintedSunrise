using Flecs.NET.Core;
using Godot;

namespace Systems.Bridge
{
    public static class PhysicsBridge
    {
        public static void Setup(World world)
        {
            world.System<Components.Core.Transform, Components.Physics.Velocity, Components.Physics.Gravity, Components.Character.MovementStats>()
                .Kind(Ecs.OnUpdate)
                .Each((Entity entity, ref Components.Core.Transform transform, ref Components.Physics.Velocity velocity, ref Components.Physics.Gravity gravity, ref Components.Character.MovementStats stats) =>
                {
                    if (!Kernel.NodeRef.TryGet(entity, out Node3D node))
                        return;

                    if (node is not CharacterBody3D charBody)
                        return;

                    // Apply gravity
                    velocity.Value.Y += gravity.Acceleration * world.DeltaTime();

                    // Sync position and velocity
                    charBody.GlobalPosition = transform.Position.ToGodot();
                    charBody.Velocity = velocity.Value.ToGodot();

                    charBody.MoveAndSlide();

                    transform.Position = (Components.Math.Vec3)charBody.GlobalPosition;
                    velocity.Value = (Components.Math.Vec3)charBody.Velocity;

                    // Ground check
                    if (entity.Has<Components.Character.Character>())
                    {
                        ref var character = ref entity.GetMut<Components.Character.Character>();
                        character.IsGrounded = charBody.IsOnFloor();
                    }

                    // Visual rotation sync (colBody equivalent)
                    if (entity.Has<Components.Character.Player>())
                    {
                        ref var player = ref entity.GetMut<Components.Character.Player>();

                        // Find child in "visual_body" group
                        Node3D visualBody = null;
                        foreach (Node child in charBody.GetChildren())
                        {
                            if (child is Node3D node3d && node3d.IsInGroup("visual_body"))
                            {
                                visualBody = node3d;
                                break;
                            }
                        }

                        if (visualBody != null)
                        {
                            Vector3 rot = visualBody.Rotation;
                            rot.Y = Mathf.LerpAngle(
                                rot.Y,
                                player.CurrentRotationY,
                                stats.TurnSpeed * world.DeltaTime()
                            );
                            visualBody.Rotation = rot;
                        }
                    }
                });
        }
    }
}
