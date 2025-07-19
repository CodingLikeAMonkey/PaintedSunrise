using Godot;

namespace Kernel;

public partial class InputHandler : Node
{
    public static Vector2 MouseDelta { get; private set; }
    public static Vector2 MousePosition { get; private set; }
    public static int MouseWheel { get; private set; }
    public static bool LeftPressed { get; private set; }
    public static bool LeftReleased { get; private set; }
    public static bool RightPressed { get; private set; }
    public static bool RightReleased { get; private set; }
    public static bool EscapePressed { get; private set; }
    public static bool EscapeReleased { get; private set; }
    public static bool MoveForward { get; private set; }
    public static bool MoveBackward { get; private set; }
    public static bool MoveLeft { get; private set; }
    public static bool MoveRight { get; private set; }
    public static bool MoveUp { get; private set; }
    public static bool MoveDown { get; private set; }
    public static bool Boost { get; private set; }
    public static Vector2 RightStickInputDir { get; private set; }

    public override void _Input(InputEvent @event)
    {
        switch (@event)
        {
            case InputEventMouseMotion motion:
                MouseDelta = motion.Relative;
                MousePosition = motion.Position;
                break;

            case InputEventMouseButton button:
                if (button.ButtonIndex == MouseButton.WheelUp)
                    MouseWheel++;
                else if (button.ButtonIndex == MouseButton.WheelDown)
                    MouseWheel--;

                break;

            case InputEventKey key when key.Keycode == Key.Escape && key.Pressed:
                Input.MouseMode = Input.MouseModeEnum.Visible;
                break;
        }
    }

    public override void _Process(double delta)
    {

        MoveForward = Input.IsKeyPressed(Key.W);
        MoveBackward = Input.IsKeyPressed(Key.S);
        MoveLeft = Input.IsKeyPressed(Key.A);
        MoveRight = Input.IsKeyPressed(Key.D);
        MoveUp = Input.IsKeyPressed(Key.E);
        MoveDown = Input.IsKeyPressed(Key.Q);
        Boost = Input.IsKeyPressed(Key.Shift);

        // These checks run once per frame, avoiding spam from _Input
        LeftPressed = Input.IsActionPressed("left_click");
        LeftReleased = Input.IsActionJustReleased("left_click");
        EscapePressed = Input.IsActionJustPressed("escape");
        EscapeReleased = Input.IsActionJustReleased("escape");

        // Reset delta & wheel
        MouseDelta = Vector2.Zero;
        MouseWheel = 0;

        RightStickInputDir = new Vector2(
        Input.GetActionStrength("look_right") - Input.GetActionStrength("look_left"),
        Input.GetActionStrength("look_up") - Input.GetActionStrength("look_down")
);
    }
}
