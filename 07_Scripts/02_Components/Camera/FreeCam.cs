using Godot;
using System;

namespace Components.Camera;

public struct FreeCam
{
    public float Sensitivity;
    public float DefaultVelocity;
    public float SpeedScale;
    public float BoostMultiplier;
    public float MaxSpeed;
    public float MinSpeed;

    public Math.Vec3 LastPosition;
    public Math.Vec3 LastRotation;
    public float CurrentVelocity;
    public Math.Vec3 MovementDirection;
    public bool IsBoosted;
}