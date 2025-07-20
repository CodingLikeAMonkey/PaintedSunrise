using Godot;
using Flecs.NET.Core;

namespace Systems.Bridge
{
    public static class ThirdPersonCameraBridge
    {
        public static void Setup(World world)
        {
            world.System<Components.Camera.ThirdPersonState>()
                .Kind(Ecs.PostUpdate)
                .Iter((Iter it, Field<Components.Camera.ThirdPersonState> camState) =>
                {
                    for (int i = 0; i < it.Count(); i++)
                    {
                        Entity entity = it.Entity(i);

                        if (!Kernel.CameraNodeRef.TryGet(entity, out Camera3D camNode))
                            continue;

                        Godot.SpringArm3D springArm = camNode.GetParent<SpringArm3D>();
                        if (springArm == null)
                            continue;

                        Godot.Node3D cameraRoot = springArm.GetParent<Node3D>();
                        if (cameraRoot == null)
                            continue;

                        Godot.Vector3 rootRot = cameraRoot.RotationDegrees;
                        rootRot.Y = camState[i].rotationDegrees.Y;  // yaw is rotationDegrees.Y
                        cameraRoot.RotationDegrees = rootRot;

                        Godot.Vector3 pitchRot = springArm.RotationDegrees;
                        pitchRot.X = Mathf.Clamp(camState[i].rotationDegrees.X, -90f, 35f); // pitch is rotationDegrees.X
                        springArm.RotationDegrees = pitchRot;
                    }
                });
        }
    }
}
