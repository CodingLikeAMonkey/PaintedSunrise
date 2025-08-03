using Flecs.NET.Core;
using Godot;
using Components.Core;
using Components.Math;
using Components.Camera;

namespace Entities.Camera;

public partial class CamFreeEntity : Camera3D
{
    [Export] public ComponentResource[] Components;

    private Entity cameraEntity;

    public override void _Ready()
    {
        cameraEntity = Kernel.EcsWorld.Instance.Entity();
            // .Add<CameraComponent>();
        foreach (var comp in Components)
        {
            comp?.ApplyToEntity(cameraEntity, this);
        }

        Kernel.NodeRef<Camera3D>.Register(cameraEntity, this);
    }

    public override void _Process(double delta)
    {
        if (!cameraEntity.IsAlive()) return;

        ref TransformComponent transform = ref cameraEntity.GetMut<TransformComponent>();
        GlobalPosition = transform.Position;
        GlobalRotation = transform.Rotation;
    }
}
