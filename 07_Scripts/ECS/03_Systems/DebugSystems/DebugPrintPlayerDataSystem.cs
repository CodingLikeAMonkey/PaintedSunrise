using Flecs.NET.Core;
using Kernel;
using Components.Character;
using Components.Core;

namespace Systems.Debug;

public static class DebugPrintPlayerDataSystem
{
    public static void Setup(World world)
    {
        world.System<CharacterComponent, CharacterPlayerComponent, TransformComponent, CharacterLastPositionComponent>()
            .Kind(Ecs.OnUpdate)
            .Each((ref CharacterComponent character, ref CharacterPlayerComponent player, ref TransformComponent transform, ref CharacterLastPositionComponent last) =>
            {
                Log.Info("Transform: " + transform.Position.ToString() + " Last Positon: " + last.Value);
            });

    }
}
