using Flecs.NET.Core;
using Godot;

namespace Systems.Debug;
public static class PrintStaticMeshes
{
    private static float _timer = 0;

    public static void Setup(World world)
    {
        world.System()
            .Kind(Ecs.OnUpdate)
            .Iter((Iter it) =>
            {
                float delta = it.DeltaTime();
                _timer += delta;

                // Print every 2 seconds
                if (_timer >= 2.0f)
                {
                    PrintSM(world);
                    _timer = 0;
                }
            });
    }

    private static void PrintSM(World world)
    {
        var query = world.Query<Components.Mesh.Static, Components.Core.Transform>();
        GD.Print("===== Static Meshes =====");

        query.Each((Entity e, ref Components.Mesh.Static mesh, ref Components.Core.Transform t) =>
        {
            string name = mesh.Node?.Name ?? "Unnamed Mesh";
            GD.Print($"- {name}:");
            GD.Print($"  Position: {t.Position}");
            GD.Print($"  Rotation: {t.Rotation}");
            GD.Print($"  Scale: {t.Scale}");
            GD.Print($"  Type: {mesh.MeshType}");
        });

        GD.Print("=========================");
    }
}