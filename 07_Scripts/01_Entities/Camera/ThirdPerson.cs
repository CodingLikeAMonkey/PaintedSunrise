using Godot;
using Flecs.NET.Core;
using System;

namespace Entities.Camera;

public partial class ThirdPerson : Node3D
{
    private Entity entity;

    public override void _Ready()
    {
        entity = Kernel.EcsWorld.Instance.Entity()
        .Set(new Components.Camera.ThirdPerson
        {
            Node = this,
        })
        .Set(new Components.Core.Transform
        {
            Position = GlobalPosition,
            Rotation = GlobalRotation,
            Scale = Scale
        });
    }

}
