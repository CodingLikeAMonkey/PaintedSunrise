using Godot;

namespace Kernel;
public partial class InputHandler : Node
{
    public static Vector2 MouseDelta { get; private set; }
    public static Vector2 MousePosition { get; private set; }
    public static int MouseWheel { get; private set; }

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
                else if (button.ButtonIndex == MouseButton.Right)
                {
                    if (button.Pressed)
                    {
                        // Capture mouse on right-click press
                        Input.MouseMode = Input.MouseModeEnum.Captured;
                    }
                    else
                    {
                        // Release mouse on right-click release
                        Input.MouseMode = Input.MouseModeEnum.Visible;
                    }
                }
                break;

            // Add escape key to release mouse
            case InputEventKey key when key.Keycode == Key.Escape && key.Pressed:
                Input.MouseMode = Input.MouseModeEnum.Visible;
                break;
        }
    }

    public override void _Process(double delta)
    {
        // Reset frame-specific values
        MouseDelta = Vector2.Zero;
        MouseWheel = 0;
    }
}