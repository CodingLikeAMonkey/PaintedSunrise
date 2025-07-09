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
        if (SkipEntityCreation) return;
        
        _entity = Kernel.EcsWorld.Instance.Entity()
            .Set(new Components.Mesh.Static { MeshType = MeshType })
            .Set(new Components.Core.Transform
            {
                Position = (Components.Math.Vec3)GlobalPosition,
                Rotation = (Components.Math.Vec3)GlobalRotation,
                Scale = (Components.Math.Vec3)Scale
            })
            .Set(new Components.Mesh.LOD
            {
                Lod1ScenePath = Kernel.Utility.GetUnifiedLOD1Path(SceneFilePath),
                OriginalScenePath = SceneFilePath,
                CameraDistance = LOD1Distance
            });

        // Register node reference
        Kernel.NodeRef.Register(_entity, this);
    }

    public override void _ExitTree()
    {
        if (!SkipEntityCreation && _entity.IsAlive())
        {
            Kernel.NodeRef.Unregister(_entity);
        }
    }
}