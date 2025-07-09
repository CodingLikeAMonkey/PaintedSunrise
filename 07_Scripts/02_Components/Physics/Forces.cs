using System.ComponentModel.Design.Serialization;

namespace Components.Physics;

public struct Forces
{
    public Components.Math.Vec3 PendingForce;
    public Components.Math.Vec3 PendingImpulse;
}
