using Flecs.NET.Core;
using Godot;
using Components.Core;
using Components.Math;
using Components.Character;
using Components.Physics;
using Components.Input;

namespace Entities.Character
{
    public partial class CharacterEntity : CharacterBody3D
    {
        private Entity characterEntity;
        private Entity visualBodyEntity;

        public override void _Ready()
        {
            var world = Kernel.EcsWorld.Instance;

            // Create main character ECS entity
            characterEntity = world
                .Entity()
                .Set(new TransformComponent
                {
                    Position = (Vec3Component)GlobalPosition,
                    Rotation = (Vec3Component)GlobalRotation,
                    Scale = (Vec3Component)Scale
                })
                .Set(new CharacterStateComponent { })
                .Set(new CharacterMovementStatsComponent { })
                .Set(new PhysicsGravityComponent { })
                .Set(new InputDeadZoneComponent { })

                .Add<CharacterComponent>()
                .Add<PhysicsVelocityComponent>()
                .Add<PhysicsColliderComponent>();

            Kernel.NodeRef.Register(characterEntity, this);

            // Find the child node with group "visual_body" and create a separate ECS entity for it
            foreach (Node child in GetChildren())
            {
                if (child is Node3D node3D && node3D.IsInGroup("visual_body"))
                {
                    // Create ECS entity for visual body
                    visualBodyEntity = world
                        .Entity()
                        .Set(new TransformComponent
                        {
                            Position = (Vec3Component)node3D.GlobalPosition,
                            Rotation = (Vec3Component)node3D.GlobalRotation,
                            Scale = (Vec3Component)node3D.Scale
                        });

                    Kernel.NodeRef.Register(visualBodyEntity, node3D);

                    break; // Stop after first visual_body found
                }
            }
        }
    }
}
