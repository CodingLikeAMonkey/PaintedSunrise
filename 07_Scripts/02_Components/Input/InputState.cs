// Components/InputState.cs

using System.ComponentModel;

namespace Components.Input
{
    public struct InputState
    {
        public Components.Math.Vec2 MouseDelta;
        public Components.Math.Vec2 MousePosition;
        public int MouseWheel;
        public bool LeftPressed;
        public bool LeftReleased;
        public bool RightPressed;
        public bool RightReleased;
        public bool EscapePressed;
        public bool EscapeReleased;
        public bool MoveForward;
        public bool MoveBackward;
        public bool MoveLeft;
        public bool MoveRight;
        public bool MoveUp;
        public bool MoveDown;
        public bool Boost;
        public Components.Math.Vec2 RightStickInputDir;
        public Components.Math.Vec2 LeftStickInputDir;
    }
}