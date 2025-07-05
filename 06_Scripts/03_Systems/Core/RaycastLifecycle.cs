#nullable enable
using Flecs.NET.Core;
using Godot;
using System;

namespace Systems.Core;

public static class RaycastLifecycle
{
    public static void Setup(World world)
    {
        world.System<Components.Core.Raycast>()
            .Kind(Ecs.OnUpdate)
            .Each((ref Components.Core.Raycast rayComponent) =>
            {
                // Try to find existing RayCast3D child node
                RayCast3D? raycast = rayComponent.Node.GetNodeOrNull<RayCast3D>("Raycast");

                if (raycast == null)
                {
                    // Create and add it only once
                    raycast = new RayCast3D();
                    raycast.Name = "Raycast";  // Give it a unique name for later retrieval
                    rayComponent.Node.AddChild(raycast);
                }

                // Update RayCast3D properties based on component data
                raycast.TargetPosition = rayComponent.Direction.Normalized() * rayComponent.Length;
                raycast.CollisionMask = rayComponent.CollisionMask;
                raycast.Position = rayComponent.Offset; // Offset relative to parent Node3D
                raycast.Enabled = true;
                raycast.Visible = rayComponent.DebugDraw;

                // Perform the raycast query
                raycast.ForceRaycastUpdate();

                // Update component hit info from RayCast3D results
                rayComponent.Hit = raycast.IsColliding();
                if (rayComponent.Hit)
                {
                    rayComponent.HitPosition = raycast.GetCollisionPoint();
                    rayComponent.HitNormal = raycast.GetCollisionNormal();
                    rayComponent.HitNode = raycast.GetCollider() as Node3D;
                }
                else
                {
                    rayComponent.HitPosition = Vector3.Zero;
                    rayComponent.HitNormal = Vector3.Zero;
                    rayComponent.HitNode = null;
                }
            });
    }
}
