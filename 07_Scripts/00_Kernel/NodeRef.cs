// Kernel/NodeRef.cs
using System.Collections.Generic;
using Flecs.NET.Core;
using Godot;

namespace Kernel
{
    public static class NodeRef
    {
        private static readonly Dictionary<Entity, Node3D> Map = new();
        
        public static void Register(Entity entity, Node3D node) => Map[entity] = node;
        public static void Unregister(Entity entity) => Map.Remove(entity);
        public static bool TryGet(Entity entity, out Node3D node) => Map.TryGetValue(entity, out node);
        public static void Update(Entity entity, Node3D newNode) => Map[entity] = newNode;
    }
}