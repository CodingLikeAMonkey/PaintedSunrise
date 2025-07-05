using Godot;
using System;
using Flecs.NET.Core;

namespace Components.Core;

public enum MouseModeEnum
{
    Visible,
    Hidden,
    Captured,
    Confined,
    ConfinedHidden,
    Max
}

public struct MouseMode
{
    public MouseModeEnum CurrentMouseMode;

    public MouseMode()
    {
        CurrentMouseMode = MouseModeEnum.Visible;
    }
}
