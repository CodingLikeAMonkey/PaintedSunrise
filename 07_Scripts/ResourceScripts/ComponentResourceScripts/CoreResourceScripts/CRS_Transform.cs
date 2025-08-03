using Godot;
using Flecs.NET.Core;
using Components.Core;
using Components.Math;

[GlobalClass]
public partial class CRS_Transform : ComponentResource
{
    public override void ApplyToEntity(Entity entity, Node3D ownerNode)
    {
        var data = new TransformComponent
        {
            Position = (Vec3Component)ownerNode.GlobalPosition,
            Rotation = (Vec3Component)ownerNode.GlobalRotation,
            Scale = (Vec3Component)ownerNode.Scale
        };

        entity.Set(data);
    }
}
