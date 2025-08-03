using System.Security.Cryptography.X509Certificates;
using Components.Math;

namespace Components.Lighting;

public struct LightingSunComponent
{
    public float Strength;
    public Vec3Component Color;
    public bool Visible;

    public LightingSunComponent()
    {
        Strength = 1.0f;
        Color = new Vec3Component(0.961f, 0.819f, 0.768f);
        Visible = true;
    }
}