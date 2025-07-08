using Flecs.NET.Core;
using Godot;
using Components.Core;
using Components.Math;

namespace Entities.Camera;

public partial class FreeCam : Camera3D
{
    private Entity cameraEntity;

    public override void _Ready()
    {
        cameraEntity = Kernel.EcsWorld.Instance.Entity()
            .Set(new Transform
            {
                Position = (Vec3)GlobalPosition,
                Rotation = (Vec3)GlobalRotation
            })
            .Set(new Components.Camera.FreeCam
            {
                Sensitivity = 3.0f,
                DefaultVelocity = 5.0f,
                SpeedScale = 1.17f,
                BoostMultiplier = 3.0f,
                MaxSpeed = 1000f,
                MinSpeed = 0.2f,
                CurrentVelocity = 5.0f
            })
            .Add<Components.Camera.Current>();

        CameraNodeRef.Register(cameraEntity, this);
    }

    public override void _Process(double delta)
    {
        if (!cameraEntity.IsAlive()) return;

        ref Transform transform = ref cameraEntity.GetMut<Transform>();
        GlobalPosition = transform.Position;
        GlobalRotation = transform.Rotation;
    }
}
