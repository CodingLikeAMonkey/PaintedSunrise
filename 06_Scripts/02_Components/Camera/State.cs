using Godot;
using System;

namespace Components.Camera;
public struct State
{
    public float CurrentVelocity;
    public Vector3 MovementDirection;
    public bool IsBoosted;
}