using Flecs.NET.Core;
using Godot;
using Components.Core;
using Components.Math;
using Components.Camera;

namespace Entities.Camera;

public partial class CamFreeEntity : Camera3D
{
    private Entity cameraEntity;

    public override void _Ready()
    {
        cameraEntity = Kernel.EcsWorld.Instance.Entity()
            .Set(new TransformComponent
            {
                Position = (Vec3Component)GlobalPosition,
                Rotation = (Vec3Component)GlobalRotation
            })
            .Set(new CameraFreeComponent{})
            .Add<CameraComponent>();

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
