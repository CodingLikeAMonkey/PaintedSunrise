using Godot;
using System;

namespace Components.Character;

public struct Player
{
    public float WalkInputHoldTime;
    public Vector2 LastInputDirection;
    public bool WasRotatingFromTap;

    public Player()
    {
        WalkInputHoldTime = 0.0f;
        LastInputDirection = Vector2.Zero;
        WasRotatingFromTap = false;
    }
}
