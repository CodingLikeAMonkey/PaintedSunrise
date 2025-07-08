using Godot;
using Flecs.NET.Core;
using System.Collections.Generic;

public static class CameraNodeRef
{
    private static readonly Dictionary<Entity, Camera3D> map = new();

    public static void Register(Entity entity, Camera3D node)
    {
        map[entity] = node;
    }

    public static bool TryGet(Entity entity, out Camera3D node)
    {
        return map.TryGetValue(entity, out node);
    }
}