using Flecs.NET.Core;
using Godot;
using Components.Core;
using Components.Math;
using Components.Character;
using Components.Physics;
using Components.Input;
using Components.GDAP;

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
                .Set(new CharacterMovementStatsComponent { })
                .Set(new PhysicsGravityComponent { })
                .Set(new InputDeadZoneComponent { })

                .Add<CharacterComponent>()
                .Add<PhysicsVelocityComponent>()
                .Add<PhysicsColliderComponent>()

                // DAE
                .Add<DecisionIdleComponent>() .Add<ActionIdleComponent>()
                .Add<DecisionRunComponent>() .Add<ActionRunComponent>();

            Kernel.NodeRef.Register(characterEntity, this);

            foreach (Node child in GetChildren())
            {
                if (child is Node3D node3D && node3D.IsInGroup("visual_body"))
                {
                    visualBodyEntity = world
                        .Entity()
                        .Set(new TransformComponent
                        {
                            Position = (Vec3Component)node3D.GlobalPosition,
                            Rotation = (Vec3Component)node3D.GlobalRotation,
                            Scale = (Vec3Component)node3D.Scale
                        });

                    Kernel.NodeRef.Register(visualBodyEntity, node3D);

                    break;
                }
            }
        }
    }
}
