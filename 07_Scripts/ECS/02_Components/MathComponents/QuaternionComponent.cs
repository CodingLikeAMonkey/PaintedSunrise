namespace Components.Math;
public struct QuaternionComponent
{
    public float X;
        public float Y;
        public float Z;
        public float W;

        public QuaternionComponent(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public static QuaternionComponent Identity => new QuaternionComponent(0, 0, 0, 1);

        public static QuaternionComponent FromEuler(Vec3Component euler)
        {
            float cy = (float)System.Math.Cos(euler.Z * 0.5f);
            float sy = (float)System.Math.Sin(euler.Z * 0.5f);
            float cp = (float)System.Math.Cos(euler.Y * 0.5f);
            float sp = (float)System.Math.Sin(euler.Y * 0.5f);
            float cr = (float)System.Math.Cos(euler.X * 0.5f);
            float sr = (float)System.Math.Sin(euler.X * 0.5f);

            return new QuaternionComponent(
                sr * cp * cy - cr * sp * sy,
                cr * sp * cy + sr * cp * sy,
                cr * cp * sy - sr * sp * cy,
                cr * cp * cy + sr * sp * sy
            );
        }

        public static QuaternionComponent operator *(QuaternionComponent a, QuaternionComponent b)
        {
            return new QuaternionComponent(
                a.W * b.X + a.X * b.W + a.Y * b.Z - a.Z * b.Y,
                a.W * b.Y - a.X * b.Z + a.Y * b.W + a.Z * b.X,
                a.W * b.Z + a.X * b.Y - a.Y * b.X + a.Z * b.W,
                a.W * b.W - a.X * b.X - a.Y * b.Y - a.Z * b.Z
            );
        }

        public Vec3Component Rotate(Vec3Component vec)
        {
            // Rotate a vector by this quaternion
            QuaternionComponent v = new QuaternionComponent(vec.X, vec.Y, vec.Z, 0);
            QuaternionComponent qInv = Inverse();
            QuaternionComponent result = this * v * qInv;
            return new Vec3Component(result.X, result.Y, result.Z);
        }

        public QuaternionComponent Inverse()
        {
            float dot = X * X + Y * Y + Z * Z + W * W;
            float invDot = dot == 0 ? 0 : 1f / dot;
            return new QuaternionComponent(-X * invDot, -Y * invDot, -Z * invDot, W * invDot);
        }

        public override string ToString()
        {
            return $"Quaternion({X}, {Y}, {Z}, {W})";
        }
}
