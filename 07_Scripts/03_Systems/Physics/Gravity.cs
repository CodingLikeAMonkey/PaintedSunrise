// Systems/Physics/Gravity.cs
using Flecs.NET.Core;
using Components.Physics;
using Components.Core;

namespace Systems.Physics;

public static class GravitySystem
{
    public static void Setup(World world)
    {
        // Create singleton entity reference
        Entity deltaTimeEntity = world.Entity("DeltaTime");
        
        world.System<Gravity, Components.Physics.Character, Velocity>()
            .Kind(Ecs.OnUpdate)
            .Each((Entity e, ref Gravity g, ref Components.Physics.Character ch, ref Velocity v) =>
            {
                if (!g.Enabled || ch.IsOnFloor) return;
                
                // Access DeltaTime through its entity
                ref DeltaTime dt = ref deltaTimeEntity.GetMut<DeltaTime>();
                v.Linear.Y -= g.Value * dt.Value;
            });
    }
}