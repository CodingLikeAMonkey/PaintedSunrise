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
        // Instance.Component<Components.Camera.Tag>();
        // Instance.Component<Components.Core.Transform>();
        // Instance.Component<Components.Camera.Settings>();
        // Instance.Component<Components.Camera.State>();
        Instance.Component<Components.Mesh.Static>();
        // Instance.Component<Components.Shaders.Outline>();
        // Instance.Component<Components.Mesh.Selectable>();

        // Create delta time entity
        _deltaTimeEntity = Instance.Entity("DeltaTime")
            .Set(new DeltaTime { Value = 0f });

        // Create systems
        //InputSystem.Setup(Instance);
        // Systems.Input.FreeCam.Setup(Instance);
        // Systems.Camera.FreeCam.Setup(Instance);
        // Systems.Shaders.Outline.Setup(Instance);
        // Systems.Shaders.ClearOutline.Setup(Instance);
        // Systems.Core.RaycastLifecycle.Setup(Instance);
        // Systems.Camera.SelectRaycast.Setup(Instance);
        // Systems.Debug.PrintStaticMeshes.Setup(Instance);
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