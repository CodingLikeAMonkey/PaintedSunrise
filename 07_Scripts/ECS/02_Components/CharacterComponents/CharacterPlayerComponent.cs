using System.Security.Cryptography.X509Certificates;
using Components.Math;

namespace Components.Character;

public struct CharacterPlayerComponent
{
    public float WalkInputHoldTime;
    public Vec2Component LastInputDirection;
    public bool WasRotatingFromTap;
    public bool HasInput;
    public Vec3Component LastMoveDir;

    public CharacterPlayerComponent()
    {
        WalkInputHoldTime = 0.0f;
        LastInputDirection = Vec2Component.Zero;
        WasRotatingFromTap = false;
        HasInput = false;
        LastMoveDir = Vec3Component.Zero;
    }
}
