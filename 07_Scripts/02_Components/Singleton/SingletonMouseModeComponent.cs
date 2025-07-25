namespace Components.Singleton;

public enum MouseModeEnum
{
    Visible,
    Hidden,
    Captured,
    Confined,
    ConfinedHidden,
    Max
}
public struct SingletonMouseModeComponent
{
    public MouseModeEnum CurrentMouseMode;

    public SingletonMouseModeComponent()
    {
        CurrentMouseMode = MouseModeEnum.Visible;
    }
}
