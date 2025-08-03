using Godot;
using Components.Core;
using Flecs.NET.Core;
using Components.Math;
using Components.Lighting;
using Kernel;

public partial class DirectionalLightEntity : Node3D
{
    private Entity entity;

    public override void _Ready()
    {
        var world = Kernel.EcsWorld.Instance;

        entity = world
            .Entity()
            .Set(new TransformComponent
            {
                Position = (Vec3Component)GlobalPosition,
                Rotation = (Vec3Component)GlobalRotation,
                Scale = (Vec3Component)Scale
            })
            .Set(new LightingSunComponent { })
            .Set(new LightingMoonComponent { })
            .Set(new LightingDirectionalFillComponent { });

        NodeRef<Node3D>.Register(entity, this);
    }
}
