using Godot;
namespace Components.Camera;
public struct Settings
{
    public float Sensitivity;
    public float DefaultVelocity;
    public float SpeedScale;
    public float BoostMultiplier;
    public float MaxSpeed;
    public float MinSpeed;

    public Vector3 LastPosition;
    public Vector3 LastRotation;
}