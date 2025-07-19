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
                Position = (Components.Math.Vec3)GlobalPosition,
                Rotation = (Components.Math.Vec3)GlobalRotation,   
                Scale = (Components.Math.Vec3)Scale
            })
            .Set(new Components.Camera.Camera
            {
                IsPreferred = true
            });
            Kernel.NodeRef.Register(entity, this);

            // Find Camera3D child and register it with same entity
            var camNode = GetNodeOrNull<Camera3D>("ThirdPersonCamera__SpringArm/ThirdPersonCamera__Camera"); // adjust path as needed
            if (camNode != null)
            {
                GD.Print(camNode.Name);
                Kernel.CameraNodeRef.Register(entity, camNode); // You need a CameraNodeRef similar to NodeRef, or reuse NodeRef if you want
            };
    }
}
