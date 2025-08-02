// Kernel/NodeRef.cs
using System.Collections.Generic;
using Flecs.NET.Core;
using Godot;

namespace Kernel
{
    public static class NodeRef<T> where T : Node
    {
        private static readonly Dictionary<Entity, T> EntityToNode = new();
        private static readonly Dictionary<T, Entity> NodeToEntity = new();

        public static void Register(Entity entity, T node)
        {
            if (entity.IsAlive())
            {
                EntityToNode[entity] = node;
                NodeToEntity[node] = entity;
            }
        }

        public static void Unregister(Entity entity)
        {
            if (EntityToNode.TryGetValue(entity, out T node))
            {
                EntityToNode.Remove(entity);
                NodeToEntity.Remove(node);
            }
        }

        public static bool TryGet(Entity entity, out T node) =>
            EntityToNode.TryGetValue(entity, out node);

        public static bool TryGetFromNode(T node, out Entity entity) =>
            NodeToEntity.TryGetValue(node, out entity);

        public static void Update(Entity entity, T newNode)
        {
            if (EntityToNode.TryGetValue(entity, out T oldNode))
            {
                NodeToEntity.Remove(oldNode);
            }

            EntityToNode[entity] = newNode;
            NodeToEntity[newNode] = entity;
        }
    }
}
