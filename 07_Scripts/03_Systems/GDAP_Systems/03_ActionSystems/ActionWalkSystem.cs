using Flecs.NET.Core;
using Components.GDAP;
using Components.Character;
using Components.Physics;
using Kernel;
using Components.State;
namespace Systems.GDAP;
public static class ActionWalkSystem
{
    public static void Setup(World world)
    {
        world.System<CharacterComponent, PhysicsVelocityComponent, CharacterMovementStatsComponent>()
            .Kind(Ecs.OnUpdate)
            .MultiThreaded()
            .Iter((Iter it, Field<CharacterComponent> c, Field<PhysicsVelocityComponent> v, Field<CharacterMovementStatsComponent> s) =>
            {
                for (int i = 0; i < it.Count(); i++)
                {
                    Entity entity = it.Entity(i);
                    var character = c[i];
                    var velocity = v[i];
                    ref var stats = ref s[i];

                    if (stats.CurrentSpeed == stats.WalkSpeed)
                    {
                        entity.Add<StateCharacterWalk>();

                    }
                    else if (stats.CurrentSpeed == stats.Speed)
                    {
                        entity.Add<StateCharacterRun>();
                    }

                }

            });
    }
}