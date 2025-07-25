using Components.Math;

namespace Components.Character;

public struct CharacterPlayerComponent
{
    public float WalkInputHoldTime;
    public Vec2Component LastInputDirection;
    public bool WasRotatingFromTap;
    public bool HasInput;

    public CharacterPlayerComponent()
    {
        WalkInputHoldTime = 0.0f;
        LastInputDirection = Vec2Component.Zero;
        WasRotatingFromTap = false;
        HasInput = false;
    }
}
