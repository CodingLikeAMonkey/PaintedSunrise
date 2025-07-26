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

        public override void _Ready()
        {
            characterEntity = Kernel.EcsWorld.Instance
                .Entity()
                .Set(new TransformComponent
                {
                    Position = (Vec3Component)GlobalPosition,
                    Rotation = (Vec3Component)GlobalRotation,
                    Scale = (Vec3Component)Scale
                })
                .Set(new CharacterStateComponent {})
                .Set(new CharacterMovementStatsComponent {})
                .Set(new PhysicsGravityComponent {})
                .Set(new InputDeadZoneComponent {})

                .Add<CharacterComponent>()
                .Add<PhysicsVelocityComponent>()
                .Add<PhysicsColliderComponent>();

            Kernel.NodeRef.Register(characterEntity, this);
        }
    }
}
