using Flecs.NET.Core;
using Components.Character;
using Components.Physics;
using Components.Math;
using Components.GDAP;

namespace Systems.GDAP;

public static class ActionIdleSystem
{
    public static void Setup(World world)
    {
        world.System<CharacterComponent, DecisionIdleComponent, PhysicsVelocityComponent, CharacterStateComponent>()
            .Kind(Ecs.OnUpdate)
            .MultiThreaded()
            .Iter((Iter it, Field<CharacterComponent> c, Field<DecisionIdleComponent> dI, Field<PhysicsVelocityComponent> v, Field<CharacterStateComponent> cs) =>
            {
                for (int i = 0; i < it.Count(); i++)
                {
                    var character = c[i];
                    var decisionIdle = dI[i];
                    var velocity = v[i];
                    ref var characterState = ref cs[i];

                    if (character.IsGrounded && velocity.Value == Vec3Component.Zero)
                    {
                        characterState.CurrentState = CharacterStateEnum.IdleState;
                    }
                }
            });
    }
}