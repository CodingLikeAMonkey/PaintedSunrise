using Flecs.NET.Core;
using Godot;
using Components.Core;
using Components.Math;
using Components.Character;
using Components.Physics;

namespace Entities.Character
{
    public partial class CharacterEntity : CharacterBody3D
    {
        private Entity _characterEntity;

        public override void _Ready()
        {
            _characterEntity = Kernel.EcsWorld.Instance
                .Entity()
                .Set(new TransformComponent
                {
                    Position = (Vec3Component)GlobalPosition,
                    Rotation = (Vec3Component)GlobalRotation,
                    Scale = (Vec3Component)Scale
                })
                .Set(new CharacterComponent())
                .Set(new CharacterStateComponent())
                .Set(new CharacterMovementStatsComponent())
                .Set(new PhysicsVelocityComponent
                {
                    Value = new Vec3Component(0f, 0f, 0f)
                })
                .Set(new PhysicsGravityComponent
                {
                    Acceleration = -9.81f
                })
                .Add<PhysicsColliderComponent>();

            Kernel.NodeRef.Register(_characterEntity, this);
        }
    }
}
