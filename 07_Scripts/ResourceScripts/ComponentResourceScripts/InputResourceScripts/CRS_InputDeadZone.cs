using Godot;
using Flecs.NET.Core;
using Components.Input;
using System;

[GlobalClass]
public partial class CRS_InputDeadZone : ComponentResource
{
    [Export] private float InputDeadzone = 0.001f;


    public override void ApplyToEntity(Entity entity, Node3D ownerNode)
    {
        var data = new InputDeadZoneComponent
        {
            Value = InputDeadzone
        };
        entity.Set(data);
    }
}
