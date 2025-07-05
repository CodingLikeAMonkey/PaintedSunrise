using Flecs.NET.Core;
using Godot;

namespace Systems.Input;
public static class FreeCam
{
    public static void Setup(World world)
    {
        world.System<Components.Camera.State>()
            .Kind(Ecs.OnUpdate)
            .Each((ref Components.Camera.State state) =>
            {
                // Keyboard input
                state.MovementDirection = new Vector3(
                    Godot.Input.IsKeyPressed(Key.D) ? 1 : Godot.Input.IsKeyPressed(Key.A) ? -1 : 0,
                    Godot.Input.IsKeyPressed(Key.E) ? 1 : Godot.Input.IsKeyPressed(Key.Q) ? -1 : 0,
                    Godot.Input.IsKeyPressed(Key.S) ? 1 : Godot.Input.IsKeyPressed(Key.W) ? -1 : 0
                ).Normalized();

                state.IsBoosted = Godot.Input.IsKeyPressed(Key.Shift);
            });
    }
}