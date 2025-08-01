using Flecs.NET.Core;
using Components.Character;
using Components.Physics;
using Components.Math;

public static class CharacterStateSystem
{
    public static void Setup(World world)
    {
        world.System<CharacterStateComponent, PhysicsVelocityComponent, CharacterComponent, CharacterMovementStatsComponent>()
            .Kind(Ecs.OnUpdate)
            .MultiThreaded()
            .Iter((Iter it, Field<CharacterStateComponent> cs, Field<PhysicsVelocityComponent> v, Field<CharacterComponent> c, Field<CharacterMovementStatsComponent> ms) =>
            {
                for (int i = 0; i < it.Count(); i++)
                {
                    ref var characterState = ref cs[i];
                    var velocity = v[i];
                    var character = c[i];
                    var movementStats = ms[i];

                    // grounded
                    if (character.IsGrounded)
                    {
                        // idle
                        if (velocity.Value == Vec3Component.Zero)
                        {
                            characterState.CurrentState = CharacterStateEnum.IdleState;
                        }
                        // movement
                        else
                        {
                            // walkk
                            if (movementStats.CurrentSpeed == movementStats.WalkSpeed)
                            {
                                characterState.CurrentState = CharacterStateEnum.WalkState;
                            }

                            // run
                            else if (movementStats.CurrentSpeed == movementStats.Speed)
                            {
                                characterState.CurrentState = CharacterStateEnum.RunState;
                            }
                        }
                    }

                    // air
                    else
                    {
                        // fall
                        if (velocity.Value.Y < 0)
                        {
                            characterState.CurrentState = CharacterStateEnum.FallState;
                        }
                        // jump
                        else if (velocity.Value.Y > 0)
                        {
                            characterState.CurrentState = CharacterStateEnum.JumpState;
                        }

                    }
                }

            });
    }
}