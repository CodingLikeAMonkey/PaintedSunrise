using Godot;
using Flecs.NET.Core;

namespace Systems.Bridge
{
    public static class SetCurrentCamera
    {
        public static void Setup(World world)
        {
            world.System<Components.Camera.Camera>()
                .Kind(Ecs.PostUpdate)
                .Each((Entity entity, ref Components.Camera.Camera camera) =>
                {
                    if (Kernel.CameraNodeRef.TryGet(entity, out Camera3D camNode))
                    {
                        if (camera.IsPreferred  == true)
                        {
                            camNode.Current = true;
                        }
                        if (camNode.IsCurrent())
                        {
                            if (!entity.Has<Components.Camera.Current>())
                                entity.Add<Components.Camera.Current>();
                        }
                        else
                        {
                            if (entity.Has<Components.Camera.Current>())
                                entity.Remove<Components.Camera.Current>();
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
