using Godot;
using Flecs.NET.Core;

namespace Entities.Meshes;

public partial class Static : Node3D
{
    // Base mesh properties
    [Export] public string MeshType = "Environment";
    [Export] public bool CastShadows = true;
    [Export] public bool ReceiveShadows = true;
    [Export] public bool SkipEntityCreation { get; set; } = false;

    private Entity _entity;

    public override void _Ready()
    {
        if (SkipEntityCreation) return;
        if (string.IsNullOrEmpty(SceneFilePath))
        {
            GD.PrintErr("Static entity has no scene file path!");
            return;
        }
        string scenePath = SceneFilePath;
        string lod1Path = Kernel.Utility.GetUnifiedLOD1Path(scenePath);
        // GD.Print(lod1Path);

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
            })
            .Set(new Components.Mesh.LOD
            {
                Lod1ScenePath = lod1Path,
                Lod1Packed = GD.Load<PackedScene>(lod1Path),
                OriginalScenePath = scenePath,
                OriginalPacked = GD.Load<PackedScene>(scenePath)
            });
    }
}