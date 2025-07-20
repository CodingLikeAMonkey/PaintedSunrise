using Godot;
using Flecs.NET.Core;
using Components.Input;
using Components.Math;

namespace Kernel
{
    public partial class InputHandler : Node
    {
        // Static cached input state properties
        private static Vec2 MouseDelta = Vec2.Zero;
        private static Vec2 MousePosition = Vec2.Zero;
        private static int MouseWheel;
        private static bool LeftPressed;
        private static bool LeftReleased;
        private static bool RightPressed;
        private static bool RightReleased;
        private static bool EscapePressed;
        private static bool EscapeReleased;
        private static bool MoveForward;
        private static bool MoveBackward;
        private static bool MoveLeft;
        private static bool MoveRight;
        private static bool MoveUp;
        private static bool MoveDown;
        private static bool Boost;
        private static Vec2 RightStickInputDir = Vec2.Zero;

        private Entity? inputEntity;

        public override void _Ready()
        {
            inputEntity = Kernel.EcsWorld.InputEntity;  // reference the same singleton entity
        }

        public override void _Input(InputEvent @event)
        {
            switch (@event)
            {
                case InputEventMouseMotion motion:
                    MouseDelta = new Vec2(motion.Relative.X, motion.Relative.Y);
                    MousePosition = new Vec2(motion.Position.X, motion.Position.Y);
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
            // Update frame input states
            MoveForward = Input.IsActionPressed("up");
            MoveBackward = Input.IsActionPressed("down");
            MoveLeft = Input.IsActionPressed("left");
            MoveRight = Input.IsActionPressed("right");
            MoveUp = Input.IsKeyPressed(Key.E);
            MoveDown = Input.IsKeyPressed(Key.Q);
            Boost = Input.IsKeyPressed(Key.Shift);

            LeftPressed = Input.IsActionPressed("left_click");
            LeftReleased = Input.IsActionJustReleased("left_click");
            EscapePressed = Input.IsActionJustPressed("escape");
            EscapeReleased = Input.IsActionJustReleased("escape");

            RightStickInputDir = new Vec2(
                Input.GetActionStrength("look_right") - Input.GetActionStrength("look_left"),
                Input.GetActionStrength("look_up") - Input.GetActionStrength("look_down")
            );

            // Update ECS singleton input component with current input states
            if (inputEntity.HasValue)
            {
                var state = new InputState
                {
                    MouseDelta = MouseDelta,
                    MousePosition = MousePosition,
                    MouseWheel = MouseWheel,
                    LeftPressed = LeftPressed,
                    LeftReleased = LeftReleased,
                    RightPressed = RightPressed,
                    RightReleased = RightReleased,
                    EscapePressed = EscapePressed,
                    EscapeReleased = EscapeReleased,
                    MoveForward = MoveForward,
                    MoveBackward = MoveBackward,
                    MoveLeft = MoveLeft,
                    MoveRight = MoveRight,
                    MoveUp = MoveUp,
                    MoveDown = MoveDown,
                    Boost = Boost,
                    RightStickInputDir = RightStickInputDir
                };

                inputEntity.Value.Set(state);
            }

            // Reset delta & wheel for next frame
            MouseDelta = Vec2.Zero;
            MouseWheel = 0;
        }
    }
}
