using Flecs.NET.Core;
using Kernel;
using Components.Mesh;
using Components.Core;

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
        var query = world.Query<Static, Transform>();
        Log.PrintInfo("===== Static Meshes =====");

        query.Each((Entity e, ref Static mesh, ref Transform t) =>
        {
            // Fixed: Use e.Id property instead of method
            Log.PrintInfo($"- Entity {e.Id}:");
            Log.PrintInfo($"  Position: {t.Position}");
            Log.PrintInfo($"  Rotation: {t.Rotation}");
            Log.PrintInfo($"  Scale: {t.Scale}");
            Log.PrintInfo($"  Type: {mesh.MeshType}");
        });

        Log.PrintInfo("=========================");
    }
}