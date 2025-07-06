using Godot;
using Flecs.NET.Core;
using System;

namespace Systems.Camera;

public partial class ThirdPerson
{
    public static void Setup(World world)
    {
        world.System<Components.Core.Transform, Components.Camera.ThirdPerson>()
        .Kind(Ecs.OnUpdate)
        .Each((ref Components.Core.Transform trans, ref Components.Camera.ThirdPerson thirdPerson) =>
        {
            float delta = world.Entity("DeltaTime").Get<Kernel.DeltaTime>().Value;


            if (Godot.Input.MouseMode == Godot.Input.MouseModeEnum.Captured) {
                
            }

        });
    }
}
