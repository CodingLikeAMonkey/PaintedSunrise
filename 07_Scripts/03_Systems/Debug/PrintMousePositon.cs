using Flecs.NET.Core;
using Kernel;

namespace Systems.Debug;

public static class PrintMousePosition
{
    public static void Setup(World world, Entity inputEntity)
    {
        world.System()
            .Kind(Ecs.OnUpdate)
            .Iter((Iter iter) =>
            {
                var inputState = inputEntity.Get<Components.Input.InputState>();
                Log.Info(inputState.MousePosition.ToString());
            });
    }
}