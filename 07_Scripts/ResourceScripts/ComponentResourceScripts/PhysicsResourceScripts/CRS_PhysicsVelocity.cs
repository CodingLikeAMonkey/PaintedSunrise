using Godot;
using Flecs.NET.Core;
using Components.Physics;
using System;

[GlobalClass]
public partial class CRS_PhysicsVelocity : ComponentResource
{
    public override void ApplyToEntity(Entity entity, Node3D ownerNode)
    {
        var data = new PhysicsVelocityComponent
        {

        };
        entity.Set(data);
    }
}
