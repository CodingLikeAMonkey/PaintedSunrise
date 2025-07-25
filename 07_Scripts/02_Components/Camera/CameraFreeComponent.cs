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
}