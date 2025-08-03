using Flecs.NET.Core;
using Kernel;
using Components.Input;

namespace Systems.Debug;

public static class DebugPrintMousePositionSystem
{
    public static void Setup(World world, Entity inputEntity)
    {
        world.System()
            .Kind(Ecs.OnUpdate)
            .Iter((Iter iter) =>
            {
                var inputState = inputEntity.Get<InputStateComponent>();
                Log.Info(inputState.MousePosition.ToString());
            });
    }
}