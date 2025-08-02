// Kernel/ControlNodeRef.cs
using System.Collections.Generic;
using Flecs.NET.Core;
using Godot;

namespace Kernel
{
    public static class ControlNodeRef
    {
        private static readonly Dictionary<Entity, Control> EntityToNode = new();
        private static readonly Dictionary<Control, Entity> NodeToEntity = new();

        public static void Register(Entity entity, Control node)
        {
            if (entity.IsAlive())
            {
                EntityToNode[entity] = node;
                NodeToEntity[node] = entity;
                // GD.Print($"ControlNodeRef: Registered entity {entity.Id} with {node.Name}");
            }
        }

        public static void Unregister(Entity entity)
        {
            if (EntityToNode.TryGetValue(entity, out Control node))
            {
                EntityToNode.Remove(entity);
                NodeToEntity.Remove(node);
                // GD.Print($"ControlNodeRef: Unregistered entity {entity.Id}");
            }
        }

        public static bool TryGet(Entity entity, out Control node) =>
            EntityToNode.TryGetValue(entity, out node);

        public static bool TryGetFromNode(Control node, out Entity entity) =>
            NodeToEntity.TryGetValue(node, out entity);

        public static void Update(Entity entity, Control newNode)
        {
            if (EntityToNode.TryGetValue(entity, out Control oldNode))
            {
                NodeToEntity.Remove(oldNode);
            }

            EntityToNode[entity] = newNode;
            NodeToEntity[newNode] = entity;
        }
    }
}
