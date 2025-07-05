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

    public Vector3 LastPosition;
    public Vector3 LastRotation;
    public float CurrentVelocity;
    public Vector3 MovementDirection;
    public bool IsBoosted;
}