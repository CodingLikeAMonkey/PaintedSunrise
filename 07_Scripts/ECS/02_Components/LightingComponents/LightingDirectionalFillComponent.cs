using Components.Math;

namespace Components.Lighting;

public struct LightingDirectionalFillComponent
{
    public float Strength;
    public Vec3Component Color;
    public bool Visible;

    public LightingDirectionalFillComponent()
    {
        Strength = 0.2f;
        Color = new Vec3Component(0.678f, 0.689f, 0.69f);
        Visible = true;
    }

}