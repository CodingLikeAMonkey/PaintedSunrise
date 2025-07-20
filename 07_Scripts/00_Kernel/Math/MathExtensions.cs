using System;
using System.Numerics;

namespace Kernel.Math;

public static class MathExtensions
{
    public static Quaternion QuaternionFromEuler(Vector3 euler)
    {
        float c1 = (float)System.Math.Cos(euler.Y * 0.5f);
        float s1 = (float)System.Math.Sin(euler.Y * 0.5f);
        float c2 = (float)System.Math.Cos(euler.X * 0.5f);
        float s2 = (float)System.Math.Sin(euler.X * 0.5f);
        float c3 = (float)System.Math.Cos(euler.Z * 0.5f);
        float s3 = (float)System.Math.Sin(euler.Z * 0.5f);

        return new Quaternion(
            s1 * s2 * c3 + c1 * c2 * s3,
            s1 * c2 * c3 + c1 * s2 * s3,
            c1 * s2 * c3 - s1 * c2 * s3,
            c1 * c2 * c3 - s1 * s2 * s3
        );
    }

    public static float PosMod(float value, float mod)
    {
        return (value % mod + mod) % mod;
    }

    public static float WrapAngleDeg(float angle)
    {
        return PosMod(angle + 180f, 360f);
    }

    public static float Atan2(float y, float x)
    {
        return (float)System.Math.Atan2(y, x);
    }


    public static float WrapAngleRadPositive(float angle)
    {
        return PosMod(angle + (float)System.Math.PI, 2f * (float)System.Math.PI);
    }

    public static float WrapAngleRadSigned(float angle)
    {
        angle = PosMod(angle + MathF.PI, 2f * MathF.PI);
        if (angle < 0)
            angle += 2f * MathF.PI;
        return angle - MathF.PI;
    }

    public static float LerpAngle(float from, float to, float t)
    {
        float difference = WrapAngleRadSigned(to - from);
        return from + difference * t;
    }
}
