using Flecs.NET.Core;
using Godot;

namespace Entities.Camera;
    public partial class Camera : Camera3D
{
    private Entity _cameraEntity;

    public override void _Ready()
    {
        _cameraEntity = Kernel.EcsWorld.Instance.Entity()
            .Add<Components.Camera.Tag>()
            .Add<Components.Camera.FreeCam>()
            .Set(new Components.Core.Transform
            {
                Position = GlobalPosition,
                Rotation = GlobalRotation
            })
            .Set(new Components.Camera.Settings
            {
                Sensitivity = 3.0f,
                DefaultVelocity = 5.0f,
                SpeedScale = 1.17f,
                BoostMultiplier = 3.0f,
                MaxSpeed = 1000f,
                MinSpeed = 0.2f
            })
            .Set(new Components.Camera.State
            {
                CurrentVelocity = 5.0f
            })
            .Set(new Components.Core.Raycast
            {
                Node = this,
                Direction = Vector3.Forward,
                Length = 20000000000.0f,
                DebugDraw = true

            })
            .Add<Components.Camera.SelectRaycast>();
    }

    public override void _Process(double delta)
    {
        if (!_cameraEntity.IsAlive()) return;

        // Get mutable reference to transform
        ref Components.Core.Transform transform = ref _cameraEntity.GetMut<Components.Core.Transform>();
        GlobalPosition = transform.Position;
        GlobalRotation = transform.Rotation;
    }
}