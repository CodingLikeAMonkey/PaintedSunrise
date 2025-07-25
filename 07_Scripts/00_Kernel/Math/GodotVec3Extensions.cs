// namespace Kernel.Math;

using Components.Math;
using Godot;

public static class GodotVec3Extensions
{
    public static Vector3 ToGodot(this Vec3Component v) => new Vector3(v.X, v.Y, v.Z);
    public static Vec3Component ToVec3(this Vector3 v) => new Vec3Component(v.X, v.Y, v.Z);
}
