using Godot;
using Flecs.NET.Core;

namespace Entities.Meshes;
public partial class Static : Node3D
{
    // Base mesh properties
    [Export] public string MeshType = "Environment";
    [Export] public bool CastShadows = true;
    [Export] public bool ReceiveShadows = true;

    private Entity _entity;

    public override void _Ready()
    {
        _entity = Kernel.EcsWorld.Instance.Entity()
            .Set(new global::Components.Mesh.Static
            {
                Node = this,
                MeshType = MeshType
            })
            //.Set(new Outline())
            .Set(new Components.Core.Transform
            {
                Position = GlobalPosition,
                Rotation = GlobalRotation,
                Scale = Scale
            })
            .Set(new Components.Core.Raycast
            {
                Node = this,
                Direction = Vector3.Up,
                Length = 20f,
                DebugDraw = true
            });
    }
}