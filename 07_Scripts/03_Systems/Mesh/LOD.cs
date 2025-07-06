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
            if (distanceSquared > lod.CameraDistance)
            {
                // GD.Print("be a low bob" + staticMesh.Node.Name);
            }
            else
            {
                // GD.Print("highest fidelity" + staticMesh.Node.Name);
            }

        });
    }

}
