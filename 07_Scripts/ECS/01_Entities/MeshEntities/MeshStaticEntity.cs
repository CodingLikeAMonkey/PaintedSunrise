using Godot;
using Flecs.NET.Core;
using Components.Math;
using Components.Core;
using Components.Mesh;

namespace Entities.Meshes;

public partial class MeshStaticEntity : Node3D
{
    // Base mesh properties
     [Export] public string OriginalSceneResourcePath { get; set; } = "";
    [Export] public float LOD1Distance { get; set; } = 60.0f;
    [Export] public string MeshType = "Environment";
    [Export] public bool CastShadows = true;
    [Export] public bool ReceiveShadows = true;
    [Export] public bool SkipEntityCreation { get; set; } = false;

    private Entity _entity;

     public override void _Ready()
    {
        // ALWAYS register the node, even if skipping entity creation
        if (!Kernel.NodeRef<Node3D>.TryGetFromNode(this, out _))
        {
            _entity = Kernel.EcsWorld.Instance.Entity();
            Kernel.NodeRef<Node3D>.Register(_entity, this);
            // GD.Print($"Registered node {Name} with entity {_entity.Id}");
        }

        if (SkipEntityCreation) return;

        // Determine original scene path
        string originalPath = !string.IsNullOrEmpty(OriginalSceneResourcePath) 
            ? OriginalSceneResourcePath 
            : GetOriginalScenePath();
        
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

    private string GetOriginalScenePath()
    {
        // Traverse up the ownership hierarchy to find the original scene
        Node current = this;
        while (current != null)
        {
            if (!string.IsNullOrEmpty(current.SceneFilePath))
            {
                return current.SceneFilePath;
            }
            current = current.Owner;
        }
        return SceneFilePath; // Fallback
    }

    public override void _ExitTree()
    {
        if (Kernel.NodeRef<Node3D>.TryGetFromNode(this, out Entity entity))
        {
            Kernel.NodeRef<Node3D>.Unregister(entity);
            // GD.Print($"Unregistered entity {entity.Id}");
        }
    }
}