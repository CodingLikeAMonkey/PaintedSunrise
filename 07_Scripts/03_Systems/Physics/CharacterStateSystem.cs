// Systems/Physics/CharacterState.cs
using Flecs.NET.Core;
using Components.Physics;

namespace Systems.Physics;

public static class CharacterStateSystem
{
    public static void Setup(World world)
    {
        world.System<Components.Physics.Character>()
            .Kind(Ecs.OnValidate) // Runs after physics bridge
            .Each((ref Components.Physics.Character ch) =>
            {
                // Sync Godot physics state to ECS logic state
                ch.IsOnFloor = ch.GodotIsOnFloor;
                ch.FloorNormal = ch.GodotFloorNormal;
                
                // Reset bridge state
                ch.GodotIsOnFloor = false;
                ch.GodotFloorNormal = Components.Math.Vec3.Zero;
            });
    }
}