using System;
using Godot;

namespace Components.Math
{
    public struct Vec3 : IEquatable<Vec3>
    {
        public float X;
        public float Y;
        public float Z;

        public Vec3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        // Implicit conversion from Vec3 to Godot.Vector3
        public static implicit operator Vector3(Vec3 v)
        {
            return new Vector3(v.X, v.Y, v.Z);
        }

        // Explicit conversion from Godot.Vector3 to Vec3
        public static explicit operator Vec3(Vector3 v)
        {
            return new Vec3(v.X, v.Y, v.Z);
        }

        // Equality
        public override bool Equals(object obj)
        {
            return obj is Vec3 other && Equals(other);
        }

        public bool Equals(Vec3 other)
        {
            return X == other.X && Y == other.Y && Z == other.Z;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }

        public static bool operator ==(Vec3 left, Vec3 right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Vec3 left, Vec3 right)
        {
            return !(left == right);
        }

        // Zero vector
        public static readonly Vec3 Zero = new Vec3(0, 0, 0);

        // ToString
        public override string ToString()
        {
            return $"Vec3({X}, {Y}, {Z})";
        }

        // Math operators
        public static Vec3 operator +(Vec3 a, Vec3 b)
        {
            return new Vec3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vec3 operator -(Vec3 a, Vec3 b)
        {
            return new Vec3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Vec3 operator *(Vec3 a, float scalar)
        {
            return new Vec3(a.X * scalar, a.Y * scalar, a.Z * scalar);
        }

        public static Vec3 operator /(Vec3 a, float scalar)
        {
            return new Vec3(a.X / scalar, a.Y / scalar, a.Z / scalar);
        }

        // Normalize
        public Vec3 Normalized()
        {
            float length = Length();
            return length == 0 ? Zero : this / length;
        }

        public float Length()
        {
            return Mathf.Sqrt(X * X + Y * Y + Z * Z);
        }

        // Dot product
        public float Dot(Vec3 other)
        {
            return X * other.X + Y * other.Y + Z * other.Z;
        }

        // Cross product
        public Vec3 Cross(Vec3 other)
        {
            return new Vec3(
                Y * other.Z - Z * other.Y,
                Z * other.X - X * other.Z,
                X * other.Y - Y * other.X
            );
        }

        public float DistanceSquaredTo(Vec3 other)
        {
            float dx = X - other.X;
            float dy = Y - other.Y;
            float dz = Z - other.Z;
            return dx * dx + dy * dy + dz * dz;
        }
    }
}
