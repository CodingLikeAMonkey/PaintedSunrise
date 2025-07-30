using Flecs.NET.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Systems.Core
{
    public static class MutuallyExclusiveComponentsSystem
    {
        private static readonly Dictionary<string, HashSet<ulong>> _groups = new();

        private static bool _isProcessing = false;

        public static void RegisterGroup(World world, string groupName, params Type[] componentTypes)
        {
            if (world == null) throw new ArgumentNullException(nameof(world));
            if (string.IsNullOrEmpty(groupName)) throw new ArgumentException("Group name must not be null or empty", nameof(groupName));
            if (componentTypes == null || componentTypes.Length == 0) throw new ArgumentException("Must specify at least one component type", nameof(componentTypes));

            var componentIds = new HashSet<ulong>();

            // Get component IDs for each type
            foreach (var type in componentTypes)
            {
                var method = typeof(World).GetMethod("Id", Array.Empty<Type>());
                var genericMethod = method.MakeGenericMethod(type);
                ulong id = (ulong)genericMethod.Invoke(world, null);
                componentIds.Add(id);
            }

            _groups[groupName] = componentIds;

            // Build filter string like "(id1, id2, id3)" to mean OR of components
            string filter = "(" + string.Join(", ", componentIds) + ")";

            world.Observer(filter)
                .Event(Ecs.OnAdd)
                .Each((Iter it, int i) =>
                {
                    if (_isProcessing) return;

                    try
                    {
                        _isProcessing = true;

                        ulong addedId = it.EventId();
                        string currentGroup = null;

                        foreach (var (grpName, ids) in _groups)
                        {
                            if (ids.Contains(addedId))
                            {
                                currentGroup = grpName;
                                break;
                            }
                        }

                        if (currentGroup == null) return;

                        var groupIds = _groups[currentGroup];

                        for (int index = 0; index < it.Count(); index++)
                        {
                            Entity entity = it.Entity(index);

                            foreach (ulong compId in groupIds)
                            {
                                if (compId != addedId && entity.Has(compId))
                                {
                                    entity.Remove(compId);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Godot.GD.PrintErr("Error in MutuallyExclusiveComponentsSystem observer: " + e);
                    }
                    finally
                    {
                        _isProcessing = false;
                    }
                });
        }
    }
}
