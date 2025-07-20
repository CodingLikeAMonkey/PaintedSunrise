namespace Components.Camera;

using Components.Math;
public struct Camera
{
    public bool IsPreferred;
    public Vec3 Forward;
    public Vec3 Right;

    public Camera()
    {
        IsPreferred = false;
        Forward = Vec3.Forward;
        Right = Vec3.Right;
    }
}
