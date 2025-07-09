using Godot;
using Flecs.NET.Core;

namespace Entities.Meshes;

public partial class Static : Node3D
{
    // Base mesh properties
    [Export] public float LOD1Distance { get; set; } = 60.0f;
    [Export] public string MeshType = "Environment";
    [Export] public bool CastShadows = true;
    [Export] public bool ReceiveShadows = true;
    [Export] public bool SkipEntityCreation { get; set; } = false;

    private Entity _entity;

     public override void _Ready()
    {
        // ALWAYS register the node, even if skipping entity creation
        if (!Kernel.NodeRef.TryGetFromNode(this, out _))
        {
            _entity = Kernel.EcsWorld.Instance.Entity();
            Kernel.NodeRef.Register(_entity, this);
            GD.Print($"Registered node {Name} with entity {_entity.Id}");
        }

        if (SkipEntityCreation) return;
        
        // Set components only for primary entities
        _entity.Set(new Static { MeshType = MeshType })
               .Set(new Components.Core.Transform
               {
                   Position = (Components.Math.Vec3)GlobalPosition,
                   Rotation = (Components.Math.Vec3)GlobalRotation,
                   Scale = (Components.Math.Vec3)Scale
               })
               .Set(new Components.Mesh.LOD
               {
                   OriginalScenePath = SceneFilePath,
                   CameraDistance = LOD1Distance
               });
    }

    public override void _ExitTree()
    {
        if (Kernel.NodeRef.TryGetFromNode(this, out Entity entity))
        {
            Kernel.NodeRef.Unregister(entity);
            GD.Print($"Unregistered entity {entity.Id}");
        }
    }
}