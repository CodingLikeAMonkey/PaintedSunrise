using Flecs.NET.Core;
using Components.Character;
using Components.Physics;
using Components.Math;

namespace Systems.GDAP;

public static class ActionRunSystem
{
    public static void Setup(World world)
    {
        world.System<CharacterComponent, PhysicsVelocityComponent, CharacterStateComponent, CharacterMovementStatsComponent>()
            .Kind(Ecs.OnUpdate)
            .MultiThreaded()
            .Iter((Iter it, Field<CharacterComponent> c, Field<PhysicsVelocityComponent> v, Field<CharacterStateComponent> cs, Field<CharacterMovementStatsComponent> ms) =>
            {
                for (int i = 0; i < it.Count(); i++)
                {
                    var character = c[i];
                    var velocity = v[i];
                    ref var characterState = ref cs[i];
                    var movementStats = ms[i];

                    if (character.IsGrounded && velocity.Value != Vec3Component.Zero && movementStats.CurrentSpeed == movementStats.Speed)
                    {
                        characterState.CurrentState = CharacterStateEnum.RunState;
                    }
                }

            });
    }
}

