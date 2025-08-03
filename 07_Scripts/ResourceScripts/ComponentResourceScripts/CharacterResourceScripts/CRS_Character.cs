using Godot;
using Flecs.NET.Core;
using Components.Character;
using System;

[GlobalClass]
public partial class CRS_Character : ComponentResource
{
    public override void ApplyToEntity(Entity entity, Node3D ownerNode)
    {
        var data = new CharacterComponent
        {

        };
        entity.Set(data);
    }
}
