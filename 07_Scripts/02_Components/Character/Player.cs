using Components.Math;

namespace Components.Character;

public struct Player
{
    public float WalkInputHoldTime;
    public Vec2 LastInputDirection;
    public bool WasRotatingFromTap;
    public bool HasInput;

    public Player()
    {
        WalkInputHoldTime = 0.0f;
        LastInputDirection = Vec2.Zero;
        WasRotatingFromTap = false;
        bool HasInput = false;
    }
}
