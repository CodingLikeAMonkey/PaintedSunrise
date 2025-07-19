using Flecs.NET.Core;
using Godot;

namespace Entities.Character
{
    public partial class Player : CharacterBody3D
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
                })
                .Add<Components.Physics.Collider>();

            Kernel.NodeRef.Register(_characterEntity, this);
        }
    }
}
