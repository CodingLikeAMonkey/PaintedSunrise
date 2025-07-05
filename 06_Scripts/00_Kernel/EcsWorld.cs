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
        Instance.Component<Components.Mesh.Static>();
        Instance.Component<Components.Core.MouseMode>();
        // Create delta time entity
        _deltaTimeEntity = Instance.Entity("DeltaTime")
            .Set(new DeltaTime { Value = 0f });

        // Create systems
        Systems.Core.MouseMode.Setup(Instance);
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