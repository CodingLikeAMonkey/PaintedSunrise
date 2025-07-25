using System;
using Godot;

namespace Components.Math
{
    public struct Vec3Component : IEquatable<Vec3Component>
    {
        public float X;
        public float Y;
        public float Z;

        public Vec3Component(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        // Static vectors
        public static readonly Vec3Component Zero = new Vec3Component(0, 0, 0);
        public static readonly Vec3Component One = new Vec3Component(1, 1, 1);
        public static readonly Vec3Component UnitX = new Vec3Component(1, 0, 0);
        public static readonly Vec3Component UnitY = new Vec3Component(0, 1, 0);
        public static readonly Vec3Component UnitZ = new Vec3Component(0, 0, 1);
        public static readonly Vec3Component Up = new Vec3Component(0, 1, 0);
        public static readonly Vec3Component Down = new Vec3Component(0, -1, 0);
        public static readonly Vec3Component Left = new Vec3Component(-1, 0, 0);
        public static readonly Vec3Component Right = new Vec3Component(1, 0, 0);
        public static readonly Vec3Component Forward = new Vec3Component(0, 0, -1);
        public static readonly Vec3Component Backward = new Vec3Component(0, 0, 1);

        // Implicit conversion from Vec3 to Godot.Vector3
        public static implicit operator Vector3(Vec3Component v) => new Vector3(v.X, v.Y, v.Z);

        // Explicit conversion from Godot.Vector3 to Vec3Component
        public static explicit operator Vec3Component(Vector3 v) => new Vec3Component(v.X, v.Y, v.Z);

        // Equality
        public override bool Equals(object obj) => obj is Vec3Component other && Equals(other);
        public bool Equals(Vec3Component other) => X == other.X && Y == other.Y && Z == other.Z;

        public override int GetHashCode() => HashCode.Combine(X, Y, Z);

        public static bool operator ==(Vec3Component left, Vec3Component right) => left.Equals(right);
        public static bool operator !=(Vec3Component left, Vec3Component right) => !(left == right);

        // Tolerant equality
        public bool IsNearlyEqual(Vec3Component other, float tolerance = 1e-5f)
        {
            return DistanceSquaredTo(other) < tolerance * tolerance;
        }

        // ToString
        public override string ToString() => $"Vec3({X}, {Y}, {Z})";

        // Math operators
        public static Vec3Component operator +(Vec3Component a, Vec3Component b) => new Vec3Component(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static Vec3Component operator -(Vec3Component a, Vec3Component b) => new Vec3Component(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        public static Vec3Component operator -(Vec3Component v) => new Vec3Component(-v.X, -v.Y, -v.Z);
        public static Vec3Component operator *(Vec3Component a, float scalar) => new Vec3Component(a.X * scalar, a.Y * scalar, a.Z * scalar);
        public static Vec3Component operator *(float scalar, Vec3Component a) => a * scalar;
        public static Vec3Component operator /(Vec3Component a, float scalar) => new Vec3Component(a.X / scalar, a.Y / scalar, a.Z / scalar);

        // Magnitude
        public float Length() => Mathf.Sqrt(X * X + Y * Y + Z * Z);
        public float LengthSquared() => X * X + Y * Y + Z * Z;

        // Normalize
        public Vec3Component Normalized()
        {
            float length = Length();
            return length == 0 ? Zero : this / length;
        }

        // Clamp length
        public Vec3Component ClampLength(float maxLength)
        {
            float sqrLen = LengthSquared();
            if (sqrLen > maxLength * maxLength)
            {
                float factor = maxLength / Mathf.Sqrt(sqrLen);
                return this * factor;
            }
            return this;
        }

        // Dot product
        public float Dot(Vec3Component other) => X * other.X + Y * other.Y + Z * other.Z;

        // Cross product
        public static Vec3Component Cross(Vec3Component a, Vec3Component b)
        {
            return new Vec3Component(
                a.Y * b.Z - a.Z * b.Y,
                a.Z * b.X - a.X * b.Z,
                a.X * b.Y - a.Y * b.X
            );
        }

        // Distance
        public float DistanceTo(Vec3Component other) => (this - other).Length();
        public float DistanceSquaredTo(Vec3Component other)
        {
            float dx = X - other.X;
            float dy = Y - other.Y;
            float dz = Z - other.Z;
            return dx * dx + dy * dy + dz * dz;
        }

        // Angle to
        public float AngleTo(Vec3Component other)
        {
            float dot = Dot(other);
            float lengths = Length() * other.Length();
            if (lengths == 0) return 0;
            return Mathf.Acos(Mathf.Clamp(dot / lengths, -1f, 1f));
        }

        // Projection
        public Vec3Component Project(Vec3Component onto)
        {
            float ontoLengthSq = onto.LengthSquared();
            if (ontoLengthSq == 0) return Zero;
            return onto * (Dot(onto) / ontoLengthSq);
        }

        // Reflection
        public Vec3Component Reflect(Vec3Component normal)
        {
            return this - 2f * Dot(normal) * normal;
        }

        // Lerp
        public static Vec3Component Lerp(Vec3Component a, Vec3Component b, float t)
        {
            t = Mathf.Clamp(t, 0f, 1f);
            return a + (b - a) * t;
        }

        // Transform by quaternion
        public static Vec3Component Transform(Vec3Component v, QuaternionComponent q)
        {
            // Extract axis components
            float x = v.X, y = v.Y, z = v.Z;
            float qx = q.X, qy = q.Y, qz = q.Z, qw = q.W;

            // Quaternion rotation formula
            float ix =  qw * x + qy * z - qz * y;
            float iy =  qw * y + qz * x - qx * z;
            float iz =  qw * z + qx * y - qy * x;
            float iw = -qx * x - qy * y - qz * z;

            return new Vec3Component(
                ix * qw + iw * -qx + iy * -qz - iz * -qy,
                iy * qw + iw * -qy + iz * -qx - ix * -qz,
                iz * qw + iw * -qz + ix * -qy - iy * -qx
            );
        }

        // Swizzles
        public Vector2 XY() => new Vector2(X, Y);
        public Vector2 XZ() => new Vector2(X, Z);
        public Vector2 YZ() => new Vector2(Y, Z);
    }

}
