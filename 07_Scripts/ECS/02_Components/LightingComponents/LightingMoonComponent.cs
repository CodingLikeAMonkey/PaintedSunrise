using System.Security.Cryptography.X509Certificates;
using Components.Math;

namespace Components.Lighting;

public struct LightingMoonComponent
{
    public float Strength;
    public Vec3Component Color;
    public bool Visible;

    public LightingMoonComponent()
    {
        Strength = 1;
        Color = new Vec3Component(0.267f, 0.312f, 0.318f);
        Visible = true;
    }

}