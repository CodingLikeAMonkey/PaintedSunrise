namespace Kernel.Math;

using System;

public static class Vec3Extensions
{
    public static float Length(this Components.Math.Vec3Component v) => MathF.Sqrt(v.X * v.X + v.Y * v.Y + v.Z * v.Z);

    public static Components.Math.Vec3Component Normalize(this Components.Math.Vec3Component v)
    {
        var len = v.Length();
        return len == 0 ? v : new Components.Math.Vec3Component(v.X / len, v.Y / len, v.Z / len);
    }

    public static float Dot(this Components.Math.Vec3Component a, Components.Math.Vec3Component b) => a.X * b.X + a.Y * b.Y + a.Z * b.Z;

    public static Components.Math.Vec3Component Cross(this Components.Math.Vec3Component a, Components.Math.Vec3Component b) =>
        new Components.Math.Vec3Component(
            a.Y * b.Z - a.Z * b.Y,
            a.Z * b.X - a.X * b.Z,
            a.X * b.Y - a.Y * b.X
        );
}
