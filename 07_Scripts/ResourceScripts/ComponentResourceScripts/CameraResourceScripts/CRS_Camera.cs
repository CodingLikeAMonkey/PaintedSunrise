using Godot;
using Flecs.NET.Core;
using Components.Camera;
using System;

[GlobalClass]
public partial class CRS_Camera : ComponentResource
{
    [Export] private bool IsPreferred = false;
    public override void ApplyToEntity(Entity entity, Node3D ownerNode)
    {
        var data = new CameraComponent
        {
            IsPreferred = IsPreferred
        };
        entity.Set(data);
    }
}
