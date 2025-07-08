using Godot;
using System;
using Flecs.NET.Core;

namespace Systems.Character;

public partial class Character : Node
{
    public void Setup(World world)
    {
        world.System<Components.Core.Transform, Components.Character.Character>()
        .Kind(Ecs.OnUpdate)
        .Each((ref Components.Core.Transform transform, ref Components.Character.Character character) => {

        });
    }

}
