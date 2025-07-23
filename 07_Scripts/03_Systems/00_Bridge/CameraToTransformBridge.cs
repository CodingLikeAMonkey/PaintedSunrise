// New System: Copy camera node rotation into ECS
using Flecs.NET.Core;
using Godot;
public static class CameraToTransformBridge
{
    public static void Setup(World world)
    {
        world.System<Components.Core.Transform, Components.Camera.ThirdPersonState>()
            .Kind(Ecs.OnUpdate)
            .Iter((Iter it, Field<Components.Core.Transform> transform, Field<Components.Camera.ThirdPersonState> state) =>
            {
                for (int i = 0; i < it.Count(); i++)
                {
                    Entity entity = it.Entity(i);

                    if (!Kernel.CameraNodeRef.TryGet(entity, out Camera3D camNode))
                        continue;

                    // Copy rotation from actual node into ECS
                    var basis = camNode.GlobalTransform.Basis;
                    var quat = basis.GetRotationQuaternion();

                    // transform[i] = new Components.Core.Transform
                    // {
                    //     Position = VectorExt.ToVec3(camNode.GlobalTransform.Origin),
                    //     Rotation = QuaternionExt.ToCustomQuat(quat) // Your Quaternion struct
                    // };
                }
            });
    }
}
