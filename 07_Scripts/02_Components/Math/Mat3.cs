namespace Components.Math
{
    public struct Mat3
    {
        public Vec3 X; // First column
        public Vec3 Y; // Second column
        public Vec3 Z; // Third column

        public Mat3(Vec3 x, Vec3 y, Vec3 z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vec3 Multiply(Vec3 v)
        {
            // Matrix-vector multiplication (column-major)
            return new Vec3(
                X.X * v.X + Y.X * v.Y + Z.X * v.Z,
                X.Y * v.X + Y.Y * v.Y + Z.Y * v.Z,
                X.Z * v.X + Y.Z * v.Y + Z.Z * v.Z
            );
        }

        public override string ToString()
        {
            return $"Mat3(\n  {X},\n  {Y},\n  {Z}\n)";
        }
    }
}
