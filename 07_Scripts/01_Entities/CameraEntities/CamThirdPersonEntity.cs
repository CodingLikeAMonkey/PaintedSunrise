using Godot;
using Flecs.NET.Core;
using System;
using Components.Camera;
using Components.Core;
using Components.Math;

namespace Entities.Camera;

public partial class CamThirdPersonEntity : Node3D
{
    private Entity entity;

    public override void _Ready()
    {
        entity = Kernel.EcsWorld.Instance.Entity()
            .Set(new CameraThirdPersonConfigComponent
            {
            })
            .Set(new CameraThirdPersonStateComponent
            {
                
            })
            .Set(new TransformComponent
            {
                Position = (Vec3Component)GlobalPosition,
                Rotation = (Vec3Component)GlobalRotation,   
                Scale = (Vec3Component)Scale
            })
            .Set(new CameraComponent
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
