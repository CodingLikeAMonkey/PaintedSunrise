using Godot;
using Flecs.NET.Core;
using Components.Math;
using Components.Core;
using Components.Mesh;

namespace Entities.Meshes;

public partial class MeshStaticEntity : Node3D
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
            // GD.Print($"Registered node {Name} with entity {_entity.Id}");
        }

        if (SkipEntityCreation) return;
        
        // Set components only for primary entities
        _entity.Set(new MeshStaticEntity { MeshType = MeshType })
               .Set(new TransformComponent
               {
                   Position = (Vec3Component)GlobalPosition,
                   Rotation = (Vec3Component)GlobalRotation,
                   Scale = (Vec3Component)Scale
               })
               .Set(new MeshLODComponent
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
            // GD.Print($"Unregistered entity {entity.Id}");
        }
    }
}