using Godot;
using System;
using Flecs.NET.Core;
using Components.Core;
using Components.Math;
using Components.UI;

public partial class UIInteractiveEntity : MarginContainer
{
    private Entity entity;
    public override void _Ready()
    {
        var world = Kernel.EcsWorld.Instance;
        entity = world
        .Entity()
            .Set(new Transform2DComponent
            {
                Position = (Vec2Component)GlobalPosition,
                Rotation = 0,
                Scale = (Vec2Component)Scale
            })
            .Set(new UIBoundingBoxComponent
            {
                Width = Size.X,
                Height = Size.Y
            })
            .Add<UIInteractiveComponent>();

        Kernel.NodeRef<MarginContainer>.Register(entity, this);
    }

}
