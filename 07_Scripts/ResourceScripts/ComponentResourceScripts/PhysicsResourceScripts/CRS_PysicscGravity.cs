using Godot;
using Flecs.NET.Core;
using Components.Physics;

[GlobalClass]
public partial class CRS_PysicscGravity : ComponentResource
{
    [Export] public float Acceleration = -9.81f;
    

    public override void ApplyToEntity(Entity entity, Node3D ownerNode)
    {
        var data = new PhysicsGravityComponent
        {
            Acceleration = Acceleration
        };
        entity.Set(data);
    }
}
