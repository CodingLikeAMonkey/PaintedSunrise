using Flecs.NET.Core;
using Godot;
using Kernel;

namespace Systems.Debug;

public static class PrintMousePosition
{
    public static void Setup(World world)
    {
        world.System()
            .Kind(Ecs.OnUpdate)
            .Iter((Iter iter) =>
            {
                Log.Info(InputHandler.MousePosition.ToString());
            });
    }
}