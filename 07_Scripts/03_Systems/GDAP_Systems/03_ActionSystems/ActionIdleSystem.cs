using Flecs.NET.Core;
using Components.Character;
using Components.Physics;
using Components.Math;
using Components.GDAP;
using Components.State;

namespace Systems.GDAP;

public static class ActionIdleSystem
{
    public static void Setup(World world)
    {
        world.System<CharacterComponent, DecisionIdleComponent, PhysicsVelocityComponent>()
            .Kind(Ecs.OnUpdate)
            .MultiThreaded()
            .Iter((Iter it, Field<CharacterComponent> c, Field<DecisionIdleComponent> dI, Field<PhysicsVelocityComponent> v) =>
            {
                for (int i = 0; i < it.Count(); i++)
                {
                    Entity entity = it.Entity(i);
                    var character = c[i];
                    var decisionIdle = dI[i];
                    var velocity = v[i];

                    if (character.IsGrounded && velocity.Value == Vec3Component.Zero)
                    {
                        entity.Add<ActionIdleComponent>();
                        entity.Add<StateCharacterIdle>();
                    }
                    else
                    {
                        entity.Remove<ActionIdleComponent>();
                    }
                }
            });
    }
}