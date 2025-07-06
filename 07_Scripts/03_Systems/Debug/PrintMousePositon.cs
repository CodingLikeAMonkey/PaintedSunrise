using Flecs.NET.Core;
using Godot;
using System;

namespace Systems.Debug;

public static class PrintMousePosition
{
    public static void Setup(World world)
    {
        world.System()
            .Kind(Ecs.OnUpdate)
            .Iter((Iter iter) =>
            {
                GD.Print(Kernel.InputHandler.MousePosition);
            });
    }
}