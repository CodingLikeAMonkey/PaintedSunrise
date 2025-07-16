using Flecs.NET.Core;
using Godot;

namespace Entities.Character
{
    public partial class Character : CharacterBody3D
    {
        private Entity _characterEntity;

        public override void _Ready()
        {
            _characterEntity = Kernel.EcsWorld.Instance
                .Entity()
                .Set(new Components.Core.Transform
                {
                    Position = (Components.Math.Vec3)GlobalPosition,
                    Rotation = (Components.Math.Vec3)GlobalRotation,
                    Scale = (Components.Math.Vec3)Scale
                })
                .Add<Components.Character.Player>()
                .Set(new Components.Character.Character())
                .Set(new Components.Character.State())
                .Set(new Components.Character.MovementStats())
                .Set(new Components.Physics.Velocity
                {
                    Value = new Components.Math.Vec3(0f, 0f, 0f)
                })
                .Set(new Components.Physics.Gravity
                {
                    Acceleration = -9.81f
                });

            Kernel.NodeRef.Register(_characterEntity, this);
        }

        public override void _PhysicsProcess(double delta)
        {

            var veloComp = _characterEntity.Get<Components.Physics.Velocity>();
            Vector3 currentVel = (Vector3)veloComp.Value;

            float gravityAccel = _characterEntity.Get<Components.Physics.Gravity>().Acceleration;
            currentVel.Y += gravityAccel * (float)delta;

            Velocity = currentVel;
            MoveAndSlide();
            Vector3 postVel = Velocity;

            _characterEntity.Set(new Components.Physics.Velocity
            {
                Value = (Components.Math.Vec3)postVel
            });
            var tx = _characterEntity.Get<Components.Core.Transform>();
            tx.Position = (Components.Math.Vec3)GlobalPosition;
            tx.Rotation = (Components.Math.Vec3)GlobalRotation;
            _characterEntity.Set(tx);

            var state = _characterEntity.Get<Components.Character.Character>();
            state.IsGrounded = IsOnFloor();
            _characterEntity.Set(state);
        }

    }
}
