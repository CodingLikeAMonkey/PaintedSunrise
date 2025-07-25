using System;

namespace Components.Math
{
    public static class MathUtils
    {
        public static float MoveToward(float current, float target, float maxDelta)
        {
            float diff = target - current;

            if (MathF.Abs(diff) <= maxDelta)
                return target;

            return current + MathF.Sign(diff) * maxDelta;
        }
    }
}

