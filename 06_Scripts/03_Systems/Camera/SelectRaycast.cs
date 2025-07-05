using Flecs.NET.Core;
using Godot;
using Entities.Meshes;
using Components.Core;

namespace Systems.Camera
{
    public static class SelectRaycast
    {
        private static void DrawDebugRay(Node3D parent, Vector3 from, Vector3 to, Color color, float duration = 0.05f)
        {
            var lineMesh = new ImmediateMesh();
            var mat = new StandardMaterial3D {
                ShadingMode = BaseMaterial3D.ShadingModeEnum.Unshaded,
                AlbedoColor = color
            };

            lineMesh.SurfaceBegin(Mesh.PrimitiveType.Lines, mat);
            lineMesh.SurfaceAddVertex(from);
            lineMesh.SurfaceAddVertex(to);
            lineMesh.SurfaceEnd();

            var inst = new MeshInstance3D {
                Mesh = lineMesh,
                CastShadow = GeometryInstance3D.ShadowCastingSetting.Off
            };
            parent.AddChild(inst);

            parent.GetTree()
                .CreateTimer(duration)
                .Timeout += () => inst.QueueFree();
        }

        public static void Setup(World world)
        {
            world.System<Components.Camera.Tag, Raycast>()
                 .Kind(Ecs.OnUpdate)
                 .Each((ref Components.Camera.Tag camTag, ref Raycast ray) =>
            {
                // defaults
                if (ray.Length <= 0) ray.Length = 100f;
                if (ray.CollisionMask == 0) ray.CollisionMask = 1 << 1;

                if (ray.Node == null || !ray.Node.IsInsideTree())
                {
                    GD.PrintErr("Raycast node missing or not in scene tree");
                    return;
                }

                // compute origin/target
                Vector3 origin, target;
                if (ray.Node is Camera3D camera)
                {
                    Vector2 mpos = Kernel.InputHandler.MousePosition;
                    Vector3 baseOrig = camera.ProjectRayOrigin(mpos);
                    Vector3 baseDir  = camera.ProjectRayNormal(mpos);

                    // apply offset in world‑space
                    var gt = ray.Node.GlobalTransform;
                    Vector3 offset = gt.Basis * ray.Offset;
                    origin = baseOrig + offset;
                    target = origin + baseDir * ray.Length;
                }
                else
                {
                    var gt = ray.Node.GlobalTransform;
                    origin = gt.Origin + gt.Basis * ray.Offset;
                    Vector3 dir = (gt.Basis * ray.Direction).Normalized();
                    target = origin + dir * ray.Length;
                }

                // do the raycast
                var space = ray.Node.GetWorld3D().DirectSpaceState;
                var query = PhysicsRayQueryParameters3D.Create(origin, target);
                query.CollisionMask   = ray.CollisionMask;
                query.CollideWithBodies = true;
                query.CollideWithAreas  = false;

                var result = space.IntersectRay(query);

                // handle hit/no‑hit
                bool hitThisFrame = result.Count > 0;
                if (hitThisFrame)
                {
                    ray.Hit          = true;
                    ray.HitPosition  = (Vector3)result["position"];
                    ray.HitNormal    = (Vector3)result["normal"];
                    ray.HitNode      = result["collider"].AsGodotObject() as Node3D;

                    // if it’s a selectable mesh, highlight it
                    if (ray.HitNode is Selectable sel && sel.Entity.IsAlive())
                    {
                        ray.HitEntity = sel.Entity;
                        if (!ray.PreviousHit)
                        {
                            GD.Print($"Hovered entity: {sel.Entity.Id}");
                            sel.Entity.Add<Components.Shaders.Outline>();
                        }
                    }
                }
                else
                {
                    ray.Hit = false;
                    // clear previous highlight
                    if (ray.PreviousHit && ray.HitEntity.IsAlive())
                    {
                        GD.Print("Exited hover");
                        ray.HitEntity.Remove<Components.Shaders.Outline>();
                    }
                    ray.HitEntity = Entity.Null();
                    ray.HitNode   = null;
                }

                ray.PreviousHit = ray.Hit;
            });
        }
    }
}
