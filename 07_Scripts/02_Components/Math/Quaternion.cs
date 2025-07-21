namespace Components.Math;

using System;

public struct Quaternion
{
    public float X;
    public float Y;
    public float Z;
    public float W;

    public Quaternion(float x, float y, float z, float w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    public static Quaternion Identity => new Quaternion(0, 0, 0, 1);

    public static Quaternion FromEuler(Vec3 euler)
    {
        float cy = (float)System.Math.Cos(euler.Z * 0.5f);
        float sy = (float)System.Math.Sin(euler.Z * 0.5f);
        float cp = (float)System.Math.Cos(euler.Y * 0.5f);
        float sp = (float)System.Math.Sin(euler.Y * 0.5f);
        float cr = (float)System.Math.Cos(euler.X * 0.5f);
        float sr = (float)System.Math.Sin(euler.X * 0.5f);

        return new Quaternion(
            sr * cp * cy - cr * sp * sy,
            cr * sp * cy + sr * cp * sy,
            cr * cp * sy - sr * sp * cy,
            cr * cp * cy + sr * sp * sy
        );
    }

    public static Quaternion operator *(Quaternion a, Quaternion b)
    {
        return new Quaternion(
            a.W * b.X + a.X * b.W + a.Y * b.Z - a.Z * b.Y,
            a.W * b.Y - a.X * b.Z + a.Y * b.W + a.Z * b.X,
            a.W * b.Z + a.X * b.Y - a.Y * b.X + a.Z * b.W,
            a.W * b.W - a.X * b.X - a.Y * b.Y - a.Z * b.Z
        );
    }

    public Vec3 Rotate(Vec3 vec)
    {
        // Rotate a vector by this quaternion
        Quaternion v = new Quaternion(vec.X, vec.Y, vec.Z, 0);
        Quaternion qInv = Inverse();
        Quaternion result = this * v * qInv;
        return new Vec3(result.X, result.Y, result.Z);
    }

    public Quaternion Inverse()
    {
        float dot = X * X + Y * Y + Z * Z + W * W;
        float invDot = dot == 0 ? 0 : 1f / dot;
        return new Quaternion(-X * invDot, -Y * invDot, -Z * invDot, W * invDot);
    }

    public override string ToString()
    {
        return $"Quaternion({X}, {Y}, {Z}, {W})";
    }
    public static Quaternion FromAxisAngle(Vec3 axis, float angle)
    {
        Vec3 normalizedAxis = axis.Normalized();
        float halfAngle = angle * 0.5f;
        float sinHalfAngle = (float)MathF.Sin(halfAngle);
        float cosHalfAngle = (float)MathF.Cos(halfAngle);

        return new Quaternion(
            normalizedAxis.X * sinHalfAngle,
            normalizedAxis.Y * sinHalfAngle,
            normalizedAxis.Z * sinHalfAngle,
            cosHalfAngle
        );
    }

    public Quaternion Normalized()
    {
        float length = MathF.Sqrt(X * X + Y * Y + Z * Z + W * W);
        if (length == 0f)
            return Quaternion.Identity;

        float invLength = 1f / length;
        return new Quaternion(
            X * invLength,
            Y * invLength,
            Z * invLength,
            W * invLength
        );
    }

    public Vec3 ToEuler()
    {
        float ysqr = Y * Y;

        float t0 = 2f * (W * X + Y * Z);
        float t1 = 1f - 2f * (X * X + ysqr);
        float roll = (float)Math.Atan2(t0, t1);

        float t2 = 2f * (W * Y - Z * X);
        t2 = Math.Clamp(t2, -1f, 1f); // or your own Clamp if needed
        float pitch = (float)Math.Asin(t2);

        float t3 = 2f * (W * Z + X * Y);
        float t4 = 1f - 2f * (ysqr + Z * Z);
        float yaw = (float)Math.Atan2(t3, t4);

        return new Vec3(roll, pitch, yaw);
    }

    public static explicit operator Quaternion(Godot.Quaternion q)
    {
        return new Quaternion(q.X, q.Y, q.Z, q.W);
    }

    public static implicit operator Godot.Quaternion(Quaternion q)
    {
        return new Godot.Quaternion(q.X, q.Y, q.Z, q.W);
    }



}
