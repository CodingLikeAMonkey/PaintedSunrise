// Kernel/NodeRef.cs
using System.Collections.Generic;
using Flecs.NET.Core;
using Godot;

namespace Kernel
{
    public static class NodeRef
    {
        private static readonly Dictionary<Entity, Node3D> EntityToNode = new();
        private static readonly Dictionary<Node3D, Entity> NodeToEntity = new();

        public static void Register(Entity entity, Node3D node)
        {
            if (entity.IsAlive())
            {
                EntityToNode[entity] = node;
                NodeToEntity[node] = entity;
                GD.Print($"NodeRef: Registered entity {entity.Id} with {node.Name}");
            }
        }
        
        public static void Unregister(Entity entity)
        {
            if (EntityToNode.TryGetValue(entity, out Node3D node))
            {
                EntityToNode.Remove(entity);
                NodeToEntity.Remove(node);
                GD.Print($"NodeRef: Unregistered entity {entity.Id}");
            }
        }
        
        public static bool TryGet(Entity entity, out Node3D node) => 
            EntityToNode.TryGetValue(entity, out node);
            
        public static bool TryGetFromNode(Node3D node, out Entity entity) => 
            NodeToEntity.TryGetValue(node, out entity);
        
        public static void Update(Entity entity, Node3D newNode)
        {
            if (EntityToNode.TryGetValue(entity, out Node3D oldNode))
            {
                NodeToEntity.Remove(oldNode);
            }
            
            EntityToNode[entity] = newNode;
            NodeToEntity[newNode] = entity;
        }
    }
}