using Flecs.NET.Core;
using Godot;
using System;

namespace Entities.Character;

public partial class Character : CharacterBody3D
{
    private Entity characterEntity;
    public override void _Ready()
    {
        characterEntity = Kernel.EcsWorld.Instance.Entity()
            .Set(new Components.Core.Transform
            {
                Position = (Components.Math.Vec3)GlobalPosition,
                Rotation = (Components.Math.Vec3)GlobalRotation,
                Scale = (Components.Math.Vec3)Scale  

            })
            .Add<Components.Character.Player>()
            .Set(new Components.Character.Character
            {
                Node = this
            })
            .Set(new Components.Character.State
            {

            })
            .Set(new Components.Character.MovementStats
            {
                
            });
    }
}
