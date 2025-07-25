using System;
using Godot;

namespace Components.Math
{
    public struct Vec2Component : IEquatable<Vec2Component>
    {
        public float X;
        public float Y;

        public Vec2Component(float x, float y)
        {
            X = x;
            Y = y;
        }

        // Implicit conversion from Vec2Component to Godot.Vector2
        public static implicit operator Vector2(Vec2Component v)
        {
            return new Vector2(v.X, v.Y);
        }

        // Explicit conversion from Godot.Vector2 to Vec2Component
        public static explicit operator Vec2Component(Vector2 v)
        {
            return new Vec2Component(v.X, v.Y);
        }

        // Equality
        public override bool Equals(object obj)
        {
            return obj is Vec2Component other && Equals(other);
        }

        public bool Equals(Vec2Component other)
        {
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public static bool operator ==(Vec2Component left, Vec2Component right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Vec2Component left, Vec2Component right)
        {
            return !(left == right);
        }

        // Zero vector
        public static readonly Vec2Component Zero = new Vec2Component(0, 0);

        // ToString
        public override string ToString()
        {
            return $"Vec2({X}, {Y})";
        }

        // Math operators
        public static Vec2Component operator +(Vec2Component a, Vec2Component b)
        {
            return new Vec2Component(a.X + b.X, a.Y + b.Y);
        }

        public static Vec2Component operator -(Vec2Component a, Vec2Component b)
        {
            return new Vec2Component(a.X - b.X, a.Y - b.Y);
        }

        public static Vec2Component operator *(Vec2Component a, float scalar)
        {
            return new Vec2Component(a.X * scalar, a.Y * scalar);
        }

        public static Vec2Component operator /(Vec2Component a, float scalar)
        {
            return new Vec2Component(a.X / scalar, a.Y / scalar);
        }

        // Normalize
        public Vec2Component Normalized()
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

        public float Dot(Vec2Component other)
        {
            return X * other.X + Y * other.Y;
        }

        public float DistanceSquaredTo(Vec2Component other)
        {
            float dx = X - other.X;
            float dy = Y - other.Y;
            return dx * dx + dy * dy;
        }
    }
}
