using System;
using Godot;

namespace Components.Math
{
    public struct Vec2 : IEquatable<Vec2>
    {
        public float X;
        public float Y;

        public Vec2(float x, float y)
        {
            X = x;
            Y = y;
        }

        // Implicit conversion from Vec2 to Godot.Vector2
        public static implicit operator Vector2(Vec2 v)
        {
            return new Vector2(v.X, v.Y);
        }

        // Explicit conversion from Godot.Vector2 to Vec2
        public static explicit operator Vec2(Vector2 v)
        {
            return new Vec2(v.X, v.Y);
        }

        // Equality
        public override bool Equals(object obj)
        {
            return obj is Vec2 other && Equals(other);
        }

        public bool Equals(Vec2 other)
        {
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public static bool operator ==(Vec2 left, Vec2 right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Vec2 left, Vec2 right)
        {
            return !(left == right);
        }

        // Zero vector
        public static readonly Vec2 Zero = new Vec2(0, 0);

        // ToString
        public override string ToString()
        {
            return $"Vec2({X}, {Y})";
        }

        // Math operators
        public static Vec2 operator +(Vec2 a, Vec2 b)
        {
            return new Vec2(a.X + b.X, a.Y + b.Y);
        }

        public static Vec2 operator -(Vec2 a, Vec2 b)
        {
            return new Vec2(a.X - b.X, a.Y - b.Y);
        }

        public static Vec2 operator *(Vec2 a, float scalar)
        {
            return new Vec2(a.X * scalar, a.Y * scalar);
        }

        public static Vec2 operator /(Vec2 a, float scalar)
        {
            return new Vec2(a.X / scalar, a.Y / scalar);
        }

        // Normalize
        public Vec2 Normalized()
        {
            float length = Length();
            return length == 0 ? Zero : this / length;
        }

        public float Length()
        {
            return Mathf.Sqrt(X * X + Y * Y);
        }

        public float LengthSquared()
        {
            return X * X + Y * Y;
        }

        public float Dot(Vec2 other)
        {
            return X * other.X + Y * other.Y;
        }

        public float DistanceSquaredTo(Vec2 other)
        {
            float dx = X - other.X;
            float dy = Y - other.Y;
            return dx * dx + dy * dy;
        }
    }
}
