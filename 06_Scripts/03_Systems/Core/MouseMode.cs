using Godot;
using Flecs.NET.Core;
using System;

namespace Systems.Core;

public partial class MouseMode
{
    public static void Setup(World world)
    {
        world.System<Components.Core.MouseMode>()
        .Kind(Ecs.OnUpdate)
        .Each((ref Components.Core.MouseMode mousemode) =>
        {
            if (mousemode.CurrentMouseMode == Components.Core.MouseModeEnum.Visible)
            {
                Godot.Input.MouseMode = Godot.Input.MouseModeEnum.Visible;
            }
            else if (mousemode.CurrentMouseMode == Components.Core.MouseModeEnum.Hidden)
            {
                Godot.Input.MouseMode = Godot.Input.MouseModeEnum.Captured;
            }
            else if (mousemode.CurrentMouseMode == Components.Core.MouseModeEnum.Captured)
            {
                Godot.Input.MouseMode = Godot.Input.MouseModeEnum.Captured;
            }
            else if (mousemode.CurrentMouseMode == Components.Core.MouseModeEnum.Confined)
            {
                Godot.Input.MouseMode = Godot.Input.MouseModeEnum.Confined;
            }
            else if (mousemode.CurrentMouseMode == Components.Core.MouseModeEnum.ConfinedHidden)
            {
                Godot.Input.MouseMode = Godot.Input.MouseModeEnum.ConfinedHidden;
            }
            else if (mousemode.CurrentMouseMode == Components.Core.MouseModeEnum.Max)
            {
                Godot.Input.MouseMode = Godot.Input.MouseModeEnum.Max;
            }

        });
    }
}
