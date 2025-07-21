using Godot;
using MathQ = Components.Math.Quaternion;  // Alias Components.Math.Quaternion to MathQ
using GodotQ = Godot.Quaternion;            // Alias Godot.Quaternion to GodotQ

namespace Kernel.Math
{
    public static class GodotQuaternionExtensions
    {
        // Converts Components.Math.Quaternion to Godot.Quaternion
        public static GodotQ ToGodot(this MathQ q)
        {
            return new GodotQ(q.X, q.Y, q.Z, q.W);
        }

        // Converts Godot.Quaternion to Components.Math.Quaternion
        public static MathQ ToMathQuaternion(this GodotQ q)
        {
            return new MathQ(q.X, q.Y, q.Z, q.W);
        }
    }
}
