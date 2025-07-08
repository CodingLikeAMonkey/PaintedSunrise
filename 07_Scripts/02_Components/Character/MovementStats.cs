namespace Components.Character;

public struct MovementStats
{
    public float WalkThreshold;
    public float TapThreshold;
    public float Speed;
    public float WalkSpeed;
    public float Acceleration;
    public float Friction;
    public float TurnSpeed;
    public float JumpImpulse;

    public MovementStats()
    {
        WalkThreshold = 0.7f;
        TapThreshold = 0.06f;
        Speed = 5.0f;
        WalkSpeed = 2.0f;
        Acceleration = 35.0f;
        Friction = 50.0f;
        TurnSpeed = 10.0f;
        JumpImpulse = 5.0f;
    }
}
