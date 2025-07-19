using Flecs.NET.Core;
using Kernel;
namespace Systems.Debug;

public static class PrintPlayerData
{
    public static void Setup(World world)
    {
        world.System<Components.Character.Character, Components.Character.Player, Components.Core.Transform>()
            .Kind(Ecs.OnUpdate)
            .Each((ref Components.Character.Character character, ref Components.Character.Player player, ref Components.Core.Transform transform) =>
            {
                Log.Info(character.IsGrounded.ToString());
            });

    }
}
