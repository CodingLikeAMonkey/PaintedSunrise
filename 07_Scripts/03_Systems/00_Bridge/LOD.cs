using System.Collections.Generic;
using Godot;
using Flecs.NET.Core;
using Kernel;
using Components.Mesh;
using Components.Core;

namespace Systems.Bridge
{
    public static class LODSystem
    {
        // Static cache to store loaded PackedScenes
        private static readonly Dictionary<string, PackedScene> _sceneCache = new();

        // Helper method: load or fetch from cache
        private static PackedScene GetCachedScene(string path)
        {
            if (!_sceneCache.TryGetValue(path, out var scene))
            {
                scene = GD.Load<PackedScene>(path);
                _sceneCache[path] = scene;
            }
            return scene;
        }

        private static bool IsSameScene(Node3D node, string scenePath)
        {
            return node.SceneFilePath == scenePath;
        }

        public static void Setup(World world)
        {
            world.System<Transform, LOD>()
                 .Kind(Ecs.PostUpdate)
                 .Iter((Iter it, Field<Transform> transField, Field<LOD> lodField) =>
                 {
                     Entity? cachedCameraEntity = null;
                     Transform cachedCameraTransform = default;

                     // Cache active camera entity & transform once per system iteration
                     if (cachedCameraEntity == null)
                     {
                         var cameraQuery = it.World().QueryBuilder<Transform>()
                             .With<Components.Camera.Current>()
                             .Build();

                         cameraQuery.Each((Entity entity, ref Transform transform) =>
                         {
                             cachedCameraEntity = entity;
                             cachedCameraTransform = transform;
                         });

                         if (cachedCameraEntity == null)
                         {
                             Log.PrintError("LOD System: No active camera found.");
                             return;
                         }
                     }

                     // Iterate over all entities with Transform and LOD components
                     foreach (int i in it)
                     {
                         ref Transform trans = ref transField[i];
                         ref LOD lod = ref lodField[i];
                         Entity entity = it.Entity(i);

                         if (!NodeRef.TryGet(entity, out Node3D currentNode))
                         {
                             Log.PrintError("LOD System: No node associated with entity");
                             continue;
                         }

                         float distanceSquared = trans.Position.DistanceSquaredTo(cachedCameraTransform.Position);
                         float thresholdSquared = lod.CameraDistance * lod.CameraDistance;

                         // Switch to LOD1 if too far
                         if (lod.CurrentLod == 0 && distanceSquared > thresholdSquared)
                         {
                             string lod1Path = Kernel.Utility.GetUnifiedLOD1Path(lod.OriginalScenePath);
                             if (!IsSameScene(currentNode, lod1Path))
                             {
                                 var parent = currentNode.GetParent();
                                 var oldNode = currentNode;
                                 var oldTransform = oldNode.GlobalTransform;

                                 var packed = GetCachedScene(lod1Path);
                                 var lod1Instance = packed.Instantiate<Node3D>();

                                 if (lod1Instance is Entities.Meshes.Static staticScript)
                                     staticScript.SkipEntityCreation = true;

                                 lod1Instance.GlobalTransform = oldTransform;
                                 parent.AddChild(lod1Instance);

                                 NodeRef.Update(entity, lod1Instance);
                                 lod.CurrentLod = 1;
                                 oldNode.QueueFree();
                             }
                         }
                         // Switch back to original LOD0 if close enough
                         else if (lod.CurrentLod == 1 && distanceSquared <= thresholdSquared)
                         {
                             if (!IsSameScene(currentNode, lod.OriginalScenePath))
                             {
                                 var parent = currentNode.GetParent();
                                 var oldNode = currentNode;
                                 var oldTransform = oldNode.GlobalTransform;

                                 var packed = GetCachedScene(lod.OriginalScenePath);
                                 var originalInstance = packed.Instantiate<Node3D>();

                                 if (originalInstance is Entities.Meshes.Static staticScript)
                                     staticScript.SkipEntityCreation = true;

                                 originalInstance.GlobalTransform = oldTransform;
                                 parent.AddChild(originalInstance);

                                 NodeRef.Update(entity, originalInstance);
                                 lod.CurrentLod = 0;
                                 oldNode.QueueFree();
                             }
                         }
                     }
                 });
        }
    }
}
