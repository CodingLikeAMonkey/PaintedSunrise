namespace Components.Camera;

using Components.Math;
public struct CameraFreeComponent
{
    public float Sensitivity;
    public float DefaultVelocity;
    public float SpeedScale;
    public float BoostMultiplier;
    public float MaxSpeed;
    public float MinSpeed;

    public Vec3Component LastPosition;
    public Vec3Component LastRotation;
    public float CurrentVelocity;
    public Vec3Component MovementDirection;
    public bool IsBoosted;

    public CameraFreeComponent()
    {
        Sensitivity = 3.0f;
        DefaultVelocity = 5.0f;
        SpeedScale = 1.17f;
        BoostMultiplier = 3.0f;
        MaxSpeed = 1000f;
        MinSpeed = 0.2f;
        CurrentVelocity = 5.0f;
    }
}