// namespace Kernel.Math;

using Components.Math;
using Godot;

public static class GodotVec3Extensions
{
    public static Vector3 ToGodot(this Vec3 v) => new Vector3(v.X, v.Y, v.Z);
    public static Vec3 ToVec3(this Vector3 v) => new Vec3(v.X, v.Y, v.Z);
}
