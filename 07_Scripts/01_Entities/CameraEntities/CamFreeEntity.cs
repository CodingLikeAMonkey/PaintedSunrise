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
            .Set(new CameraFreeComponent
            {
                Sensitivity = 3.0f,
                DefaultVelocity = 5.0f,
                SpeedScale = 1.17f,
                BoostMultiplier = 3.0f,
                MaxSpeed = 1000f,
                MinSpeed = 0.2f,
                CurrentVelocity = 5.0f
            })
            .Add<CameraComponent>();

        Kernel.CameraNodeRef.Register(cameraEntity, this);
    }

    public override void _Process(double delta)
    {
        if (!cameraEntity.IsAlive()) return;

        ref TransformComponent transform = ref cameraEntity.GetMut<TransformComponent>();
        GlobalPosition = transform.Position;
        GlobalRotation = transform.Rotation;
    }
}
