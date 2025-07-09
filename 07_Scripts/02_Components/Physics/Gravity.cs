namespace Components.Physics;

public struct Gravity
{
    public float Value; // m/s²
    public bool Enabled;
    
    public Gravity(float value = 9.8f, bool enabled = true)
    {
        Value = value;
        Enabled = enabled;
    }
}