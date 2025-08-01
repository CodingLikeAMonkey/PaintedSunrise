using Flecs.NET.Core;
using Components.Character;
using Components.Physics;
using Components.Input;
using Kernel;

namespace Systems.Character;

public static class CharacterJumpSystem
{
    public static void Setup(World world, Entity inputEntity)
    {
        world.System<CharacterStateComponent, PhysicsVelocityComponent, CharacterMovementStatsComponent>()
            .Kind(Ecs.OnUpdate)
            .MultiThreaded()
            .Iter((Iter it, Field<CharacterStateComponent> cs, Field<PhysicsVelocityComponent> v, Field<CharacterMovementStatsComponent> ms) =>
            {
                var inputState = inputEntity.Get<InputStateComponent>();
                for (int i = 0; i < it.Count(); i++)
                {
                    var characterState = cs[i];
                    ref var velocity = ref v[i];
                    var movement = ms[i];

                    if (inputState.Jump)
                    {
                        if (characterState.CurrentState == CharacterStateEnum.IdleState ||
                        characterState.CurrentState == CharacterStateEnum.WalkState ||
                        characterState.CurrentState == CharacterStateEnum.RunState)
                        {
                            velocity.Value.Y += movement.JumpImpulse;
                        }

                    }

                }

            });
    }
}