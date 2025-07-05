using Flecs.NET.Core;
using Godot;

namespace Entities.Camera;
    public partial class FreeCam : Camera3D
{
    private Entity cameraEntity;

    public override void _Ready()
    {
        cameraEntity = Kernel.EcsWorld.Instance.Entity()
            .Set(new Components.Core.Transform
            {
                Position = GlobalPosition,
                Rotation = GlobalRotation
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

            });
           
            // .Set(new Components.Core.Raycast
            // {
            //     Node = this,
            //     Direction = Vector3.Forward,
            //     Length = 20000000000.0f,
            //     DebugDraw = true

            // })
            // .Add<Components.Camera.SelectRaycast>();
    }

    public override void _Process(double delta)
    {
        if (!cameraEntity.IsAlive()) return;

        // Get mutable reference to transform
        ref Components.Core.Transform transform = ref cameraEntity.GetMut<Components.Core.Transform>();
        GlobalPosition = transform.Position;
        GlobalRotation = transform.Rotation;
    }
}