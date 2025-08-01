using Godot;
using Flecs.NET.Core;
using Kernel;
using Components.Core;
using Components.Math;
using Components.Character;

public partial class CharacterLastPositionEntity : Marker3D
{
    private Entity entity;
    public override void _Ready()
    {
        // TopLevel = true;
        // CharacterBody3D parentCharacter = GetOwner<CharacterBody3D>();
        // Entity parentCharacterEntity = NodeRef.TryGetFromNode(parentCharacter, out var bruh);


        // var world = EcsWorld.Instance;

        // entity = world
        //     .Entity()
        //     .Set(new TransformComponent
        //     {
        //         Position = (Vec3Component)GlobalPosition,
        //         Rotation = (Vec3Component)GlobalRotation,
        //         Scale = (Vec3Component)Scale

        //     })
        //     .Add<CharacterLastPositionComponent>();
        // NodeRef.Register(entity, this);
    }
}
