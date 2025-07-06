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
        .Each((ref Components.Core.Transform trans, ref Components.Mesh.LOD lod, ref Components.Mesh.Static staticMesh) =>
        {
            var cameraQuery = world.QueryBuilder<Components.Core.Transform>()
                .With<Components.Camera.Current>()
                .Build();

            Entity? cameraEntity = null;
            Components.Core.Transform cameraTransform = default;

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

            float distanceSquared = trans.Position.DistanceSquaredTo(cameraTransform.Position);
            
            // LOD0 ➜ LOD1
            if (lod.CurrentLod == 0 && distanceSquared > lod.CameraDistance * lod.CameraDistance)
            {
                if (lod.Lod1Packed != null)
                {
                    var parent = staticMesh.Node.GetParent();
                    var oldTransform = staticMesh.Node.GlobalTransform;

                    var lod1Instance = lod.Lod1Packed.Instantiate<Node3D>();
                    lod1Instance.GlobalTransform = oldTransform;
                    parent.AddChild(lod1Instance);

                    staticMesh.Node.QueueFree();
                    staticMesh.Node = lod1Instance;

                    lod.CurrentLod = 1;

                    GD.Print($"[LOD] Switched {lod1Instance.Name} to LOD1");
                }
                else
                {
                    GD.PrintErr($"[LOD] LOD1Packed was null for {staticMesh.Node.Name}");
                }
            }
            // else
            // {
            //     // GD.Print("highest fidelity" + staticMesh.Node.Name);
            // }

        });
    }

}
