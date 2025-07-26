namespace Components.Math;

using System;
public static class MathUtilComponent
{
    public const float Tau = 2 * MathF.PI;
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

     public static float LerpAngle(float from, float to, float t)
    {
        from = NormalizeAngle(from);
        to = NormalizeAngle(to);

        float difference = to - from;

        if (difference > MathF.PI)
            difference -= Tau;
        else if (difference < -MathF.PI)
            difference += Tau;

        float result = from + difference * t;
        return NormalizeAngle(result);
    }

    public static float NormalizeAngle(float angle)
    {
        angle %= Tau;
        if (angle < 0)
            angle += Tau;
        return angle;
    }

    public static float RadToDeg(float radians)
    {
        return radians * (180f / MathF.PI);
    }

    public static float Atan2(float y, float x)
    {
        return MathF.Atan2(y, x);
    }

    public static float Abs(float value)
    {
        return MathF.Abs(value);
    }

    public static float PosMod(float value, float modulus)
    {
        float result = value % modulus;
        if (result < 0)
            result += modulus;
        return result;
    }
}
