using Godot;
using Flecs.NET.Core;

namespace Systems.Bridge
{
    public static class ThirdPersonCameraBridge
    {
        public static void Setup(Flecs.NET.Core.World world)
        {
            world.System<Components.Camera.ThirdPerson>()
                .Kind(Flecs.NET.Core.Ecs.PostUpdate)
                .Iter((Flecs.NET.Core.Iter it, Flecs.NET.Core.Field<Components.Camera.ThirdPerson> cam) =>
                {
                    for (int i = 0; i < it.Count(); i++)
                    {
                        Flecs.NET.Core.Entity entity = it.Entity(i);

                        if (!Kernel.CameraNodeRef.TryGet(entity, out Godot.Camera3D camNode))
                            continue;

                        Godot.SpringArm3D springArm = camNode.GetParent<Godot.SpringArm3D>();
                        if (springArm == null)
                            continue;

                        Godot.Node3D cameraRoot = springArm.GetParent<Godot.Node3D>();
                        if (cameraRoot == null)
                            continue;

                        Godot.Vector3 rootRot = cameraRoot.RotationDegrees;
                        rootRot.Y = cam[i].rotDeg.Y;  // yaw is rotDeg.Y
                        cameraRoot.RotationDegrees = rootRot;

                        Godot.Vector3 pitchRot = springArm.RotationDegrees;
                        pitchRot.X = Mathf.Clamp(cam[i].rotDeg.X, -90f, 35f); // pitch is rotDeg.X
                        springArm.RotationDegrees = pitchRot;
                    }
                });
        }
    }
}
