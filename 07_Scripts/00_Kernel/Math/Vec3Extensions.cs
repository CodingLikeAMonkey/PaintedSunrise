namespace Kernel.Math;

using System;

public static class Vec3Extensions
{
    public static float Length(this Components.Math.Vec3 v) => MathF.Sqrt(v.X * v.X + v.Y * v.Y + v.Z * v.Z);

    public static Components.Math.Vec3 Normalize(this Components.Math.Vec3 v)
    {
        var len = v.Length();
        return len == 0 ? v : new Components.Math.Vec3(v.X / len, v.Y / len, v.Z / len);
    }

    public static float Dot(this Components.Math.Vec3 a, Components.Math.Vec3 b) => a.X * b.X + a.Y * b.Y + a.Z * b.Z;

    public static Components.Math.Vec3 Cross(this Components.Math.Vec3 a, Components.Math.Vec3 b) =>
        new Components.Math.Vec3(
            a.Y * b.Z - a.Z * b.Y,
            a.Z * b.X - a.X * b.Z,
            a.X * b.Y - a.Y * b.X
        );
}
