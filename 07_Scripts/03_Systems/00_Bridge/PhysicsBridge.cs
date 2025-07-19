using Flecs.NET.Core;
using Godot;

namespace Systems.Bridge
{
    public static class PhysicsBridge
    {
        public static void Setup(World world)
        {
            world.System<Components.Core.Transform, Components.Physics.Velocity, Components.Physics.Gravity>()
                .Kind(Ecs.OnUpdate)
                .Each((Entity entity, ref Components.Core.Transform transform, ref Components.Physics.Velocity velocity, ref Components.Physics.Gravity gravity) => 
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

                    transform.Position = (Components.Math.Vec3)body.GlobalPosition;
                    velocity.Value = (Components.Math.Vec3)body.Velocity;

                    if (entity.Has<Components.Character.Character>())
                    {
                        ref var character = ref entity.GetMut<Components.Character.Character>();
                        character.IsGrounded = body.IsOnFloor();
                    }
                });
        }
    }
}