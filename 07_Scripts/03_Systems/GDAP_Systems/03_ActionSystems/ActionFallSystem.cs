using Flecs.NET.Core;
using Components.Character;
using Components.Physics;
using Components.State;
using Components.GDAP;
using Kernel;

namespace Systems.GDAP;

public static class ActionFallsSystem
{
    public static void Setup(World world)
    {
        world.System<CharacterComponent, PhysicsVelocityComponent>()
            .Kind(Ecs.OnUpdate)
            .MultiThreaded()
            .Iter((Iter it, Field<CharacterComponent> c, Field<PhysicsVelocityComponent> v) =>
            {
                for (int i = 0; i < it.Count(); i++)
                {
                    Entity entity = it.Entity(i);
                    var character = c[i];
                    var velocity = v[i];


                    if (!character.IsGrounded && velocity.Value.Y < 0)
                    {
                        entity.Add<StateCharacterFall>();
                    }
                }

            });
    }
}