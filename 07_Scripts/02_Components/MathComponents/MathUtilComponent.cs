namespace Components.Math;

using System;
public static class MathUtilComponent
{
    public static float Clamp(float value, float min, float max)
    {
        if (value < min) return min;
        if (value > max) return max;
        return value;
    }

    public static float MoveToward(float current, float target, float maxDelta)
    {
        float diff = target - current;

        if (MathF.Abs(diff) <= maxDelta)
            return target;

        return current + MathF.Sign(diff) * maxDelta;
    }
}
