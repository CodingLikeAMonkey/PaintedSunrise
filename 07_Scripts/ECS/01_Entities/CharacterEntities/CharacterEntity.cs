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
        [Export] public ComponentResource[] Components;
        private Entity characterEntity;
        private Entity visualBodyEntity;

        public override void _Ready()
        {
            var world = Kernel.EcsWorld.Instance;

            characterEntity = world.Entity();

            foreach (var comp in Components)
            {
                comp?.ApplyToEntity(characterEntity, this);
            }

            Kernel.NodeRef<Node3D>.Register(characterEntity, this);

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

                    Kernel.NodeRef<Node3D>.Register(visualBodyEntity, node3D);

                    break;
                }
            }
        }
    }
}
