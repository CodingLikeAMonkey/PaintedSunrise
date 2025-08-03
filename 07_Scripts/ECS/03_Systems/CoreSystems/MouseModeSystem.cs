using Flecs.NET.Core;
using Components.Singleton;

namespace Systems.Core;

public partial class MouseModeSystem
{
    public static void Setup(World world)
    {
        world.System<SingletonMouseModeComponent>()
        .Kind(Ecs.OnUpdate)
        .Each((ref SingletonMouseModeComponent mousemode) =>
        {
            Godot.Input.MouseMode = ConvertToGodot(mousemode.CurrentMouseMode);
        });
    }

    private static Godot.Input.MouseModeEnum ConvertToGodot(MouseModeEnum mode)
    {
        return mode switch
        {
            MouseModeEnum.Visible => Godot.Input.MouseModeEnum.Visible,
            MouseModeEnum.Hidden => Godot.Input.MouseModeEnum.Hidden,
            MouseModeEnum.Captured => Godot.Input.MouseModeEnum.Captured,
            MouseModeEnum.Confined => Godot.Input.MouseModeEnum.Confined,
            MouseModeEnum.ConfinedHidden => Godot.Input.MouseModeEnum.ConfinedHidden,
            _ => Godot.Input.MouseModeEnum.Visible
        };
    }
}
