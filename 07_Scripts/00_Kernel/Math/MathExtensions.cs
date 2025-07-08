using System;
using System.Numerics;

public static class MathExtensions
{
    public static Quaternion QuaternionFromEuler(Vector3 euler)
    {
        float c1 = (float)Math.Cos(euler.Y * 0.5f);
        float s1 = (float)Math.Sin(euler.Y * 0.5f);
        float c2 = (float)Math.Cos(euler.X * 0.5f);
        float s2 = (float)Math.Sin(euler.X * 0.5f);
        float c3 = (float)Math.Cos(euler.Z * 0.5f);
        float s3 = (float)Math.Sin(euler.Z * 0.5f);

        return new Quaternion(
            s1 * s2 * c3 + c1 * c2 * s3,
            s1 * c2 * c3 + c1 * s2 * s3,
            c1 * s2 * c3 - s1 * c2 * s3,
            c1 * c2 * c3 - s1 * s2 * s3
        );
    }
}
