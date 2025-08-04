using System.Collections.Generic;
using Godot;
using Flecs.NET.Core;
using Kernel;
using Components.Mesh;
using Components.Core;
using Components.Camera;
using Entities.Meshes;
using Components.Time;
using Components.Singleton;

namespace Systems.Bridge
{
    public static class MeshLODBridgeSystem
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
            world.System<TransformComponent, MeshLODComponent>()
                .Kind(Ecs.PostUpdate)
                .Iter((Iter it, Field<TransformComponent> transField, Field<MeshLODComponent> lodField) =>
                {
                    float delta = world.Entity("DeltaTime").Get<SingletonDeltaTimeComponent>().Value;

                    // Cache active camera entity & transform once per system iteration
                    TransformComponent cachedCameraTransform = default;
                    bool hasCamera = false;

                    var cameraQuery = it.World().QueryBuilder<TransformComponent>()
                        .With<CameraCurrentComponent>()
                        .Build();

                    cameraQuery.Each((Entity entity, ref TransformComponent transform) =>
                    {
                        cachedCameraTransform = transform;
                        hasCamera = true;
                    });

                    foreach (int i in it)
                    {
                        ref TransformComponent trans = ref transField[i];
                        ref MeshLODComponent lod = ref lodField[i];
                        Entity entity = it.Entity(i);

                        // Create timer if missing
                        if (!entity.Has<TimerComponent>())
                        {
                            entity.Set(new TimerComponent(duration: 2.0f));
                            continue;
                        }

                        var timer = entity.Get<TimerComponent>();

                        // Update timer
                        if (timer.IsRunning)
                        {
                            timer.Elapsed += delta;

                            if (timer.Elapsed >= timer.Duration)
                            {
                                timer.Elapsed = timer.Duration;
                                timer.IsRunning = false;
                            }

                            entity.Set(timer);
                            continue;
                        }

                        // Timer finished, check for camera
                        if (!timer.ErrorShown && !hasCamera)
                        {
                            Log.PrintError("LOD System: No active camera found.");
                            timer.ErrorShown = true;
                            entity.Set(timer);
                            continue;
                        }

                        // Always update timer if modified
                        entity.Set(timer);

                        // Ensure Node3D exists
                        if (!NodeRef<Node3D>.TryGet(entity, out Node3D currentNode))
                        {
                            Log.PrintError("LOD System: No node associated with entity");
                            continue;
                        }

                        // If entity marked as "no LOD", skip
                        if (lod.CurrentLod == -1)
                            continue;

                        float distanceSquared = trans.Position.DistanceSquaredTo(cachedCameraTransform.Position);
                        float thresholdSquared = lod.CameraDistance * lod.CameraDistance;

                        // Compute LOD1 path
                        string lod1Path = Kernel.Utility.GetUnifiedLOD1Path(lod.OriginalScenePath);

                        // Check if LOD1 exists before switching
                        bool lod1Exists = FileAccess.FileExists(lod1Path);

                        if (!lod1Exists)
                        {
                            // Mark entity as no LOD and skip forever
                            lod.CurrentLod = -1;
                            continue;
                        }

                        // Switch to LOD1 if too far
                        if (lod.CurrentLod == 0 && distanceSquared > thresholdSquared)
                        {
                            if (!IsSameScene(currentNode, lod1Path))
                            {
                                var parent = currentNode.GetParent();
                                var oldNode = currentNode;
                                var oldTransform = oldNode.GlobalTransform;
                                var packed = GetCachedScene(lod1Path);
                                var lod1Instance = packed.Instantiate<Node3D>();

                                if (lod1Instance is MeshStaticEntity staticScript)
                                    staticScript.SkipEntityCreation = true;

                                lod1Instance.GlobalTransform = oldTransform;
                                parent.AddChild(lod1Instance);
                                NodeRef<Node3D>.Update(entity, lod1Instance);
                                lod.CurrentLod = 1;
                                oldNode.QueueFree();
                            }
                        }
                        // Switch back to LOD0 if close enough
                        else if (lod.CurrentLod == 1 && distanceSquared <= thresholdSquared)
                        {
                            if (!IsSameScene(currentNode, lod.OriginalScenePath))
                            {
                                var parent = currentNode.GetParent();
                                var oldNode = currentNode;
                                var oldTransform = oldNode.GlobalTransform;
                                var packed = GetCachedScene(lod.OriginalScenePath);
                                var originalInstance = packed.Instantiate<Node3D>();

                                if (originalInstance is MeshStaticEntity staticScript)
                                    staticScript.SkipEntityCreation = true;

                                originalInstance.GlobalTransform = oldTransform;
                                parent.AddChild(originalInstance);
                                NodeRef<Node3D>.Update(entity, originalInstance);
                                lod.CurrentLod = 0;
                                oldNode.QueueFree();
                            }
                        }
                    }
                });
        }
    }
}
