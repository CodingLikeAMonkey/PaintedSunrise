using Flecs.NET.Core;
using Components.UI;
using Kernel;

namespace Systems.Debug
{
    public static class DebugUIInteractiveStats
    {
        public static void Setup(World world)
        {
            world.System<UIInteractionEventComponent>()
                .Kind(Ecs.OnUpdate)
                .Iter((Iter it, Field<UIInteractionEventComponent> ie) =>
                {
                    for (int i = 0; i < it.Count(); i++)
                    {
                        Entity entity = it.Entity(i);

                        ref var evt = ref ie[i];
                        var ent = it.Entity(i);

                        if (evt.HoverEnter)
                            Log.Info("Hover enter");

                        if (evt.HoverExit)
                            Log.Info("Hover exit");

                        if (evt.Click)
                            Log.Info("Click");

                        // **Now remove the component** so it won't be seen next frame
                        entity.Remove<UIInteractionEventComponent>();
                    }
                });
        }
    }
}
