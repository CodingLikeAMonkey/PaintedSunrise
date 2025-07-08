using Flecs.NET.Core;

namespace Systems.Core;

public partial class MouseMode
{
    public static void Setup(World world)
    {
        world.System<Components.Core.Unique.MouseMode>()
        .Kind(Ecs.OnUpdate)
        .Each((ref Components.Core.Unique.MouseMode mousemode) =>
        {
            Godot.Input.MouseMode = ConvertToGodot(mousemode.CurrentMouseMode);
        });
    }

    private static Godot.Input.MouseModeEnum ConvertToGodot(Components.Core.Unique.MouseModeEnum mode)
    {
        return mode switch
        {
            Components.Core.Unique.MouseModeEnum.Visible => Godot.Input.MouseModeEnum.Visible,
            Components.Core.Unique.MouseModeEnum.Hidden => Godot.Input.MouseModeEnum.Hidden,
            Components.Core.Unique.MouseModeEnum.Captured => Godot.Input.MouseModeEnum.Captured,
            Components.Core.Unique.MouseModeEnum.Confined => Godot.Input.MouseModeEnum.Confined,
            Components.Core.Unique.MouseModeEnum.ConfinedHidden => Godot.Input.MouseModeEnum.ConfinedHidden,
            _ => Godot.Input.MouseModeEnum.Visible
        };
    }
}
