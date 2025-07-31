using Flecs.NET.Core;
using Components.GDAP;
using Components.Character;
using Components.Physics;
using Kernel;
using Components.Math;
namespace Systems.GDAP;
public static class ActionWalkSystem
{
    public static void Setup(World world)
    {
        world.System<CharacterComponent, PhysicsVelocityComponent, CharacterMovementStatsComponent, CharacterStateComponent>()
            .Kind(Ecs.OnUpdate)
            .MultiThreaded()
            .Iter((Iter it, Field<CharacterComponent> c, Field<PhysicsVelocityComponent> v, Field<CharacterMovementStatsComponent> s, Field<CharacterStateComponent> cs) =>
            {
                for (int i = 0; i < it.Count(); i++)
                {
                    Entity entity = it.Entity(i);
                    var character = c[i];
                    var velocity = v[i];
                    ref var stats = ref s[i];
                    ref var characterState = ref cs[i];

                    if (character.IsGrounded && velocity.Value != Vec3Component.Zero && stats.CurrentSpeed == stats.WalkSpeed)
                    {
                        characterState.CurrentState = CharacterStateEnum.WalkState;
                    }
                }

            });
    }
}