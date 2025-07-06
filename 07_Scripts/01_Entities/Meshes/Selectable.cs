using Flecs.NET.Core;
using Godot;
using System;

namespace Entities.Meshes;

public partial class Selectable : RigidBody3D
{
    private Entity entity;

    public Entity Entity => entity; // Public getter

    public override void _Ready()
    {
        CollisionLayer = 3;

        entity = Kernel.EcsWorld.Instance.Entity()
            .Set(new global::Components.Mesh.Selectable
            {
                Node = this,
            })
            //.Set(new Components.Shaders.Outline())
            .Set(new Components.Core.Transform
            {
                Position = GlobalPosition,
                Rotation = GlobalRotation,
                Scale = Scale
            });
    }
}

