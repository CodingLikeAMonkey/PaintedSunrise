using Godot;
using System;
using Flecs.NET.Core;

namespace Systems.Mesh;

public partial class LOD
{
    public static void Setup(World world)
{
    world.System<Components.Core.Transform, Components.Mesh.LOD, Components.Mesh.Static>()
    .Kind(Ecs.OnUpdate)
    .Iter((Iter it, Field<Components.Core.Transform> transField, Field<Components.Mesh.LOD> lodField, Field<Components.Mesh.Static> staticField) =>
    {
        // Find camera once per system run
        Entity? cameraEntity = null;
        Components.Core.Transform cameraTransform = default;

        var cameraQuery = it.World().QueryBuilder<Components.Core.Transform>()
            .With<Components.Camera.Current>()
            .Build();

        cameraQuery.Each((Entity entity, ref Components.Core.Transform transform) =>
        {
            cameraEntity = entity;
            cameraTransform = transform;
        });

        if (cameraEntity == null)
        {
            GD.PrintErr("LOD System: No active camera found.");
            return;
        }

        foreach (int i in it)
        {
            ref Components.Core.Transform trans = ref transField[i];
            ref Components.Mesh.LOD lod = ref lodField[i];
            ref Components.Mesh.Static staticMesh = ref staticField[i];

            float distanceSquared = trans.Position.DistanceSquaredTo(cameraTransform.Position);
            float thresholdSquared = lod.CameraDistance * lod.CameraDistance;

            bool IsSameScene(Node3D node, PackedScene packed) => 
                node.SceneFilePath == packed.ResourcePath;

            // Switch to LOD1
            if (lod.CurrentLod == 0 && distanceSquared > thresholdSquared)
            {
                if (lod.Lod1Packed == null)
                {
                    string lod1Path = lod.UseUnifiedLOD1
                        ? Kernel.Utility.GetUnifiedLOD1Path(lod.OriginalScenePath)
                        : Kernel.Utility.GetVariantLOD1Path(lod.OriginalScenePath);

                    lod.Lod1ScenePath = lod1Path;
                    lod.Lod1Packed = GD.Load<PackedScene>(lod1Path);

                    if (lod.Lod1Packed == null)
                    {
                        GD.PrintErr($"[LOD] Failed to load LOD1 scene: {lod1Path}");
                        continue;
                    }
                }

                if (!IsSameScene(staticMesh.Node, lod.Lod1Packed))
                {
                    var parent = staticMesh.Node.GetParent();
                    var oldNode = staticMesh.Node;
                    var oldTransform = oldNode.GlobalTransform;

                    var lod1Instance = lod.Lod1Packed.Instantiate<Node3D>();
                    
                    // Prevent entity creation in new node
                    if (lod1Instance is Entities.Meshes.Static staticScript)
                        staticScript.SkipEntityCreation = true;

                    lod1Instance.GlobalTransform = oldTransform;
                    parent.AddChild(lod1Instance);

                    staticMesh.Node = lod1Instance;
                    lod.CurrentLod = 1;
                    oldNode.QueueFree();
                }
            }
            // Switch back to original
            else if (lod.CurrentLod == 1 && distanceSquared <= thresholdSquared)
            {
                if (lod.OriginalPacked == null)
                {
                    lod.OriginalPacked = GD.Load<PackedScene>(lod.OriginalScenePath);
                    if (lod.OriginalPacked == null)
                    {
                        GD.PrintErr($"[LOD] Failed to load original scene: {lod.OriginalScenePath}");
                        continue;
                    }
                }

                if (!IsSameScene(staticMesh.Node, lod.OriginalPacked))
                {
                    var parent = staticMesh.Node.GetParent();
                    var oldNode = staticMesh.Node;
                    var oldTransform = oldNode.GlobalTransform;

                    var originalInstance = lod.OriginalPacked.Instantiate<Node3D>();
                    
                    // Prevent entity creation in new node
                    if (originalInstance is Entities.Meshes.Static staticScript)
                        staticScript.SkipEntityCreation = true;

                    originalInstance.GlobalTransform = oldTransform;
                    parent.AddChild(originalInstance);

                    staticMesh.Node = originalInstance;
                    lod.CurrentLod = 0;
                    oldNode.QueueFree();
                }
            }
        }
    });
}
}
