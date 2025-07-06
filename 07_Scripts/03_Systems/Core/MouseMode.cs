using Godot;
using Flecs.NET.Core;
using System;

namespace Systems.Core;

public partial class MouseMode
{
    public static void Setup(World world)
    {
        world.System<Components.Core.Unique.MouseMode>()
        .Kind(Ecs.OnUpdate)
        .Each((ref Components.Core.Unique.MouseMode mousemode) =>
        {
            if (mousemode.CurrentMouseMode == Components.Core.Unique.MouseModeEnum.Visible)
            {
                Godot.Input.MouseMode = Godot.Input.MouseModeEnum.Visible;
            }
            else if (mousemode.CurrentMouseMode == Components.Core.Unique.MouseModeEnum.Hidden)
            {
                Godot.Input.MouseMode = Godot.Input.MouseModeEnum.Captured;
            }
            else if (mousemode.CurrentMouseMode == Components.Core.Unique.MouseModeEnum.Captured)
            {
                Godot.Input.MouseMode = Godot.Input.MouseModeEnum.Captured;
            }
            else if (mousemode.CurrentMouseMode == Components.Core.Unique.MouseModeEnum.Confined)
            {
                Godot.Input.MouseMode = Godot.Input.MouseModeEnum.Confined;
            }
            else if (mousemode.CurrentMouseMode == Components.Core.Unique.MouseModeEnum.ConfinedHidden)
            {
                Godot.Input.MouseMode = Godot.Input.MouseModeEnum.ConfinedHidden;
            }
            else if (mousemode.CurrentMouseMode == Components.Core.Unique.MouseModeEnum.Max)
            {
                Godot.Input.MouseMode = Godot.Input.MouseModeEnum.Max;
            }

        });
    }
}
