using Flecs.NET.Core;

namespace Systems.Character;

public static class Character
{
    public static void Setup(World world)
    {
        world.System<Components.Core.Transform, Components.Character.Character>()
        .Kind(Ecs.OnUpdate)
        .MultiThreaded()
        .Iter((Iter it, Field<Components.Core.Transform> transform, Field<Components.Character.Character> character) => {

        });
    }
}
