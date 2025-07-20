namespace Components.Math
{
    public struct Basis
    {
        public Vec3 X;
        public Vec3 Y;
        public Vec3 Z;

        public Basis(Vec3 x, Vec3 y, Vec3 z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Basis FromQuaternion(Quaternion q)
        {
            // Convert quaternion to Basis (rotation matrix)
            float xx = q.X * q.X;
            float yy = q.Y * q.Y;
            float zz = q.Z * q.Z;
            float xy = q.X * q.Y;
            float xz = q.X * q.Z;
            float yz = q.Y * q.Z;
            float wx = q.W * q.X;
            float wy = q.W * q.Y;
            float wz = q.W * q.Z;

            return new Basis(
                new Vec3(1 - 2 * (yy + zz), 2 * (xy - wz), 2 * (xz + wy)),
                new Vec3(2 * (xy + wz), 1 - 2 * (xx + zz), 2 * (yz - wx)),
                new Vec3(2 * (xz - wy), 2 * (yz + wx), 1 - 2 * (xx + yy))
            );
        }

        public Vec3 Transform(Vec3 v)
        {
            return new Vec3(
                X.X * v.X + Y.X * v.Y + Z.X * v.Z,
                X.Y * v.X + Y.Y * v.Y + Z.Y * v.Z,
                X.Z * v.X + Y.Z * v.Y + Z.Z * v.Z
            );
        }

        // Operator overload for basis * vector
        public static Vec3 operator *(Basis b, Vec3 v)
        {
            return b.Transform(v);
        }
    }
}
