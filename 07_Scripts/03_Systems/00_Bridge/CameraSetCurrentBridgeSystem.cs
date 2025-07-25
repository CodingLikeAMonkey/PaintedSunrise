using Godot;
using Flecs.NET.Core;
using Components.Camera;

namespace Systems.Bridge
{
    public static class CameraSetCurrentBridgeSystem
    {
        public static void Setup(World world)
        {
            world.System<CameraComponent>()
                .Kind(Ecs.PostUpdate)
                .Each((Entity entity, ref CameraComponent camera) =>
                {
                    if (Kernel.CameraNodeRef.TryGet(entity, out Camera3D camNode))
                    {
                        if (camera.IsPreferred  == true)
                        {
                            camNode.Current = true;
                        }
                        if (camNode.IsCurrent())
                        {
                            if (!entity.Has<CameraCurrentComponent>())
                                entity.Add<CameraCurrentComponent>();
                        }
                        else
                        {
                            if (entity.Has<CameraCurrentComponent>())
                                entity.Remove<CameraCurrentComponent>();
                        }
                    }
                    else
                    {
                        // Optionally handle case where Camera3D node is not found
                    }
                });
        }
    }
}
