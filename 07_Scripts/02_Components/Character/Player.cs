using Components.Math;

namespace Components.Character;

public struct Player
{
    public float CurrentRotationY;
    public float WalkInputHoldTime;
    public Vec2 LastInputDirection;
    public bool WasRotatingFromTap;

    public Player()
    {
        WalkInputHoldTime = 0.0f;
        LastInputDirection = Vec2.Zero;
        WasRotatingFromTap = false;
    }
}
