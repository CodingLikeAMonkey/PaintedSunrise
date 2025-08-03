namespace Components.Time;

public struct TimerComponent
{
    public float Elapsed;
    public float Duration;
    public bool IsRunning;
    public bool ErrorShown;

    public TimerComponent(float duration)
    {
        Elapsed = 0.0f;
        Duration = duration;
        IsRunning = true;
        ErrorShown = false;
    }
}
