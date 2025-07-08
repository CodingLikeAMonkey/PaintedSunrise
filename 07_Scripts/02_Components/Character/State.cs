namespace Components.Character;

public enum CharacterStateEnum
{
    IdleState,
    WalkState,
    RunState,
    JumpState,
    FallState,
    LandingState,
    DodgeState
}

public struct State
{
    public CharacterStateEnum CurrentState;
    public State()
    {
        CurrentState = CharacterStateEnum.IdleState;
    }
}
