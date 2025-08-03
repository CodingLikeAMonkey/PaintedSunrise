using Godot;
using Flecs.NET.Core;
using Components.Lighting;
using Components.Core;

namespace Systems.Bridge;
public static class LightingDayNightBridgeSystem
{
    public static void Setup(World world)
    {
        world.System<TransformComponent, LightingSunComponent, LightingMoonComponent, LightingDirectionalFillComponent>()
            .Kind(Ecs.PostUpdate)
            .Each((Entity entity, ref TransformComponent t, ref LightingSunComponent s, ref LightingMoonComponent m, ref LightingDirectionalFillComponent d) =>
            {
                if (!Kernel.NodeRef<Node3D>.TryGet(entity, out var node))
                    return;

                node.GlobalRotation = t.Rotation.ToGodot();
            });
    }
}