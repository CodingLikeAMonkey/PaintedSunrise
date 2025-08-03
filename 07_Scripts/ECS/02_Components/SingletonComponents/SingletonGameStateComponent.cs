namespace Components.Singleton;

public enum GameStateEnum
{
    Debug,
    Gameplay,
    FreeCam
}

public partial class SingletonGameStateComponent
{
    public GameStateEnum CurrentGameState;

    public SingletonGameStateComponent()
    {
        CurrentGameState = GameStateEnum.Debug;
    }
}
