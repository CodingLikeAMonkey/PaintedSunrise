using Flecs.NET.Core;
using Godot;

namespace Kernel;

public partial class EcsWorld : Node
{
    public static World Instance { get; private set; }
    private Entity _deltaTimeEntity;

    public override void _Ready()
    {
        Instance = World.Create();

        // Register components
        Instance.Component<Components.Core.Transform>();
        Instance.Component<Components.Mesh.Static>();
        Instance.Component<Components.Core.Unique.MouseMode>();
        Instance.Component<Components.Core.Unique.GameState>();
        Instance.Component<Components.Camera.FreeCam>();
        Instance.Component<Components.Mesh.LOD>();


        // startup entities
        Instance.Entity("Singleton")
            .Set(new Components.Core.Unique.GameState())
            .Set(new Components.Core.Unique.MouseMode());


        // Create delta time entity
        _deltaTimeEntity = Instance.Entity("DeltaTime")
            .Set(new DeltaTime { Value = 0f });

        // Create systems
        Systems.Core.MouseMode.Setup(Instance);
        Systems.Input.FreeCam.Setup(Instance);
        Systems.Camera.FreeCamSystem.Setup(Instance);
        Systems.Core.Fsm.GameState.Setup(Instance);
        Systems.Mesh.LOD.Setup(Instance);
    }

    public override void _Process(double delta)
    {
        // Update delta time
        _deltaTimeEntity.Set(new DeltaTime { Value = (float)delta });

        // Run ECS pipeline
        Instance.Progress();
    }
}

public struct DeltaTime
{
    public float Value;
}