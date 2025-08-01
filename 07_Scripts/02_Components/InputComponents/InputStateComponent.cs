using Components.Math;

namespace Components.Input
{
    public struct InputStateComponent
    {
        public Vec2Component MouseDelta;
        public Vec2Component MousePosition;
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
        public bool Jump;
        public Vec2Component RightStickInputDir;
        public Vec2Component LeftStickInputDir;
    }
}