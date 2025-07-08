using Flecs.NET.Core;
using Kernel;

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
        Log.PrintInfo("===== Static Meshes =====");

        query.Each((Entity e, ref Components.Mesh.Static mesh, ref Components.Core.Transform t) =>
        {
            string name = mesh.Node?.Name ?? "Unnamed Mesh";
            Log.PrintInfo($"- {name}:");
            Log.PrintInfo($"  Position: {t.Position}");
            Log.PrintInfo($"  Rotation: {t.Rotation}");
            Log.PrintInfo($"  Scale: {t.Scale}");
            Log.PrintInfo($"  Type: {mesh.MeshType}");
        });

        Log.PrintInfo("=========================");
    }
}
