using Godot;
using Flecs.NET.Core;
using Components.Camera;
using Components.Math;
using System;

[GlobalClass]
public partial class CRS_CameraFree : ComponentResource
{
    [Export] private float Sensitivity = 3.0f;
    [Export] private float DefaultVelocity = 5.0f;
    [Export] private float SpeedScale = 1.17f;
    [Export] private float BoostMultiplier = 3.0f;
    [Export] private float MaxSpeed = 1000f;
    [Export] private float MinSpeed = 0.2f;
    [Export] private float CurrentVelocity = 5.0f;

    public override void ApplyToEntity(Entity entity, Node3D ownerNode)
    {
        var data = new CameraFreeComponent
        {
            Sensitivity = Sensitivity,
            DefaultVelocity = DefaultVelocity,
            SpeedScale = SpeedScale,
            BoostMultiplier = BoostMultiplier,
            MaxSpeed = MaxSpeed,
            MinSpeed = MinSpeed,
            CurrentVelocity = CurrentVelocity
        };
        entity.Set(data);
    }
}
