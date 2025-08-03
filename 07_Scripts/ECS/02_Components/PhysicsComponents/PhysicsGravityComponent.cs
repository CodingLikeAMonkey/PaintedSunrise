namespace Components.Physics;

public struct PhysicsGravityComponent
{
    public float Acceleration;

    public PhysicsGravityComponent()
    {
        Acceleration = -9.81f;
    }
}
