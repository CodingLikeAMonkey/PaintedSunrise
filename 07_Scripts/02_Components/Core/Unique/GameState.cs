namespace Components.Core.Unique;

public enum GameStateEnum
{
    Debug,
    Gameplay,
    FreeCam
}

public partial class GameState
{
    public GameStateEnum CurrentGameState;

    public GameState()
    {
        CurrentGameState = GameStateEnum.Debug;
    }
}
