using Flecs.NET.Core;
using Godot;

namespace Kernel;

public partial class EcsWorld : Node
{
    public static World Instance { get; private set; }
    private Entity _deltaTimeEntity;
    public static Entity InputEntity { get; private set; }

    public override void _Ready()
    {
        Instance = World.Create();
        Instance.SetThreads(System.Environment.ProcessorCount);

        // Register components
        Instance.Component<Components.Core.Transform>();
        Instance.Component<Components.Mesh.Static>();
        Instance.Component<Components.Core.Unique.MouseMode>();
        Instance.Component<Components.Core.Unique.GameState>();
        Instance.Component<Components.Camera.Camera>();
        Instance.Component<Components.Camera.FreeCam>();
        Instance.Component<Components.Mesh.LOD>();
        Instance.Component<Components.Physics.Gravity>();
        Instance.Component<Components.Physics.Velocity>();
        Instance.Component<Components.Input.InputState>();

        InputEntity = Instance.Entity("Singleton")
            .Set(new Components.Core.Unique.GameState())
            .Set(new Components.Core.Unique.MouseMode())
            .Set(new Components.Input.InputState());

        _deltaTimeEntity = Instance.Entity("DeltaTime")
            .Set(new Components.Core.Unique.DeltaTime { Value = 0f });

        Systems.Core.MouseMode.Setup(Instance);
        Systems.Input.FreeCamInputSystem.Setup(Instance, InputEntity);
        Systems.Camera.FreeCamSystem.Setup(Instance, InputEntity);
        Systems.Core.Fsm.GameState.Setup(Instance, InputEntity);
        Systems.Bridge.LODSystem.Setup(Instance);
        Systems.Bridge.PhysicsBridge.Setup(Instance);
        Systems.Character.Movement.Setup(Instance, InputEntity);
        Systems.Bridge.SetCurrentCamera.Setup(Instance);
        Systems.Camera.ThirdPerson.Setup(Instance, InputEntity);
        Systems.Bridge.ThirdPersonCameraBridge.Setup(Instance);

        Log.Info = GD.Print;
        Log.Warn = GD.Print;
        Log.Error = GD.Print;
    }

    public override void _Process(double delta)
    {
        _deltaTimeEntity.Set(new Components.Core.Unique.DeltaTime { Value = (float)delta });
        Instance.Progress();
    }
}