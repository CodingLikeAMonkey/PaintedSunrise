using Godot;
using Flecs.NET.Core;
using Components.Character;
using System;

[GlobalClass]
public partial class CRS_CharacterState : ComponentResource
{
    public override void ApplyToEntity(Entity entity, Node3D ownerNode)
    {
        var data = new CharacterStateComponent
        {
            
        };
        entity.Set(data);
    }
}
