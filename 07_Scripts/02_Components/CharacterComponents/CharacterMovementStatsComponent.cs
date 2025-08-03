namespace Components.Character;

public struct CharacterMovementStatsComponent
{
    public float WalkThreshold;
    public float TapThreshold;
    public float Speed;
    public float WalkSpeed;
    public float Acceleration;
    public float Friction;
    public float TurnSpeed;
    public float JumpImpulse;
    public float AirAcceleration;
    public float AirTurnSpeed;
    public float CurrentSpeed;
    public float CurrentTurnSpeed;
    public float CurrentAcceleration;

    public CharacterMovementStatsComponent()
    {
        WalkThreshold = 0.7f;
        TapThreshold = 0.06f;
        Speed = 3.0f;
        WalkSpeed = 1.2f;
        Acceleration = 25.0f;
        Friction = 35.0f;
        TurnSpeed = 10.0f;
        JumpImpulse = 5.0f;
        AirAcceleration = 0.05f;
        AirTurnSpeed = 0.1f;
        CurrentSpeed = 0.0f;
        CurrentTurnSpeed = 0.0f;
        CurrentAcceleration = 0.0f;
    }
}
