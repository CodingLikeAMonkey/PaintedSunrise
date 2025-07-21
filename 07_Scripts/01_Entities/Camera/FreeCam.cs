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
        // Convert Godot.Vector3 Euler angles (GlobalRotation) to Components.Math.Vec3
        Vec3 eulerRotation = (Vec3)GlobalRotation;

        cameraEntity = Kernel.EcsWorld.Instance.Entity()
            .Set(new Transform
            {
                Position = (Vec3)GlobalPosition,
                Rotation = Components.Math.Quaternion.FromEuler(eulerRotation),
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
            .Add<Components.Camera.Camera>();

        Kernel.CameraNodeRef.Register(cameraEntity, this);
    }

    public override void _Process(double delta)
    {
        if (!cameraEntity.IsAlive()) return;

        ref Transform transform = ref cameraEntity.GetMut<Transform>();

        GlobalPosition = transform.Position; // implicit cast Vec3 -> Godot.Vector3

        // Convert Quaternion back to Euler angles Vec3, then cast to Godot.Vector3
        Vec3 eulerFromQuat = transform.Rotation.ToEuler();
        GlobalRotation = (Godot.Vector3)eulerFromQuat;
    }
}
