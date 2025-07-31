using Flecs.NET.Core;
using Components.Character;
using Components.Physics;
using Components.GDAP;
using Kernel;

namespace Systems.GDAP;

public static class ActionFallSystem
{
    public static void Setup(World world)
    {
        world.System<CharacterComponent, PhysicsVelocityComponent, CharacterStateComponent>()
            .Kind(Ecs.OnUpdate)
            .MultiThreaded()
            .Iter((Iter it, Field<CharacterComponent> c, Field<PhysicsVelocityComponent> v, Field<CharacterStateComponent> cs) =>
            {
                for (int i = 0; i < it.Count(); i++)
                {
                    Entity entity = it.Entity(i);
                    var character = c[i];
                    var velocity = v[i];
                    ref var characterState = ref cs[i];


                    if (!character.IsGrounded && velocity.Value.Y < 0)
                    {
                        characterState.CurrentState = CharacterStateEnum.FallState;
                    }
                }

            });
    }
}