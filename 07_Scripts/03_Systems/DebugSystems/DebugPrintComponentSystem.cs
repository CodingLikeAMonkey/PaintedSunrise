using Flecs.NET.Core;
using System.Collections.Generic;
using Kernel;
using Components.Character;
using Components.GDAP;
using Components.Core;

namespace Systems.Debug
{
    public static class DebugPrintCharacterComponentsSystem
    {
        public static void Setup(World world)
        {
            world.System<CharacterComponent>()
                .Kind(Ecs.OnUpdate)
                .Iter((Iter it, Field<CharacterComponent> c) =>
                {
                    for (int i = 0; i < it.Count(); i++)
                    {
                        Entity entity = it.Entity(i);
                        PrintKnownComponents(world, entity);
                    }
                });
        }

        // You define the known components you want to check here
        private static void PrintKnownComponents(World world, Entity entity)
        {
            var attachedComponents = new List<string>();

            // Check known component types (add more as needed)
            // if (entity.Has<StateCharacterIdle>()) attachedComponents.Add(nameof(StateCharacterIdle));
            // if (entity.Has<StateCharacterFall>()) attachedComponents.Add(nameof(StateCharacterFall));
            // if (entity.Has<StateCharacterWalk>()) attachedComponents.Add(nameof(StateCharacterWalk));
            // if (entity.Has<StateCharacterRun>()) attachedComponents.Add(nameof(StateCharacterRun));
            // Add any other components you're using...

            Log.Info($"[Character Entity {entity.Id}] has components: {string.Join(", ", attachedComponents)}");
        }
    }
}
