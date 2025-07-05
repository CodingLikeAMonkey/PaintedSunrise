using Components.Core.Unique;
using Flecs.NET.Core;
using Godot;

namespace Systems.Input;

public static class FreeCam
{
    public static void Setup(World world)
    {
        world.System<Components.Camera.FreeCam>()
            .Kind(Ecs.OnUpdate)
            .Each((ref Components.Camera.FreeCam cam) =>
            {
                var singleton = world.Lookup("Singleton");
                if (singleton.IsValid())
                {
                    GD.Print("fucker found");
                    var gameState = singleton.Get<Components.Core.Unique.GameState>();
                    if (gameState.CurrentGameState == Components.Core.Unique.GameStateEnum.Gameplay)
                    {
                        // Keyboard input
                        cam.MovementDirection = new Vector3(
                            Godot.Input.IsKeyPressed(Key.D) ? 1 : Godot.Input.IsKeyPressed(Key.A) ? -1 : 0,
                            Godot.Input.IsKeyPressed(Key.E) ? 1 : Godot.Input.IsKeyPressed(Key.Q) ? -1 : 0,
                            Godot.Input.IsKeyPressed(Key.S) ? 1 : Godot.Input.IsKeyPressed(Key.W) ? -1 : 0
                        ).Normalized();

                        cam.IsBoosted = Godot.Input.IsKeyPressed(Key.Shift);
                    }
                }

            });
    }
}