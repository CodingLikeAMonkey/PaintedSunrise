using Godot;
using System;
using Flecs.NET.Core;

namespace Systems.Input;

public partial class InputHandlerSystem
{
    public static void Setup(World world)
    {
        world.System<Components.Input.Unique.MouseInput>()
        .Kind(Ecs.OnUpdate)
        .Each((ref Components.Input.Unique.MouseInput mouseInput) =>
        {

        });
    }
}
