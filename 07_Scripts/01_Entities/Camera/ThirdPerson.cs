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
            })
            .Set(new Components.Core.Transform
            {
                Position = (Components.Math.Vec3)GlobalPosition,   // <-- cast here
                Rotation = (Components.Math.Vec3)GlobalRotation,   // <-- and here
                Scale = (Components.Math.Vec3)Scale                 // <-- and here if Scale uses Vec3
            });
            // .Add<Components.Camera.Current>();
    }
}
