using Flecs.NET.Core;
using Godot;
using Components.Singleton;
using Components.Input;

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
        Instance.Component<Components.Core.TransformComponent>();
        Instance.Component<Components.Mesh.MeshStaticComponent>();
        Instance.Component<SingletonMouseModeComponent>();
        Instance.Component<SingletonGameStateComponent>();
        Instance.Component<Components.Camera.CameraComponent>();
        Instance.Component<Components.Camera.CameraFreeComponent>();
        Instance.Component<Components.Mesh.MeshLODComponent>();
        Instance.Component<Components.Physics.PhysicsGravityComponent>();
        Instance.Component<Components.Physics.PhysicsVelocityComponent>();
        Instance.Component<Components.Input.InputStateComponent>();

        InputEntity = Instance.Entity("Singleton")
            .Set(new SingletonGameStateComponent())
            .Set(new SingletonMouseModeComponent())
            .Set(new InputStateComponent());

        _deltaTimeEntity = Instance.Entity("DeltaTime")
            .Set(new SingletonDeltaTimeComponent { Value = 0f });

        Systems.Core.MouseModeSystem.Setup(Instance);
        Systems.Input.InputCameraFreeSystem.Setup(Instance, InputEntity);
        Systems.Camera.CameraFreeSystem.Setup(Instance, InputEntity);
        Systems.Core.Fsm.GameStateSystem.Setup(Instance, InputEntity);
        Systems.Bridge.MeshLODBridgeSystem.Setup(Instance);
        Systems.Bridge.CharacterPhysicsBridgeSystem.Setup(Instance);
        Systems.Character.CharacterMovementSystem.Setup(Instance, InputEntity);
        Systems.Bridge.CameraSetCurrentBridgeSystem.Setup(Instance);
        Systems.Camera.CameraThirdPersonSystem.Setup(Instance, InputEntity);
        Systems.Bridge.CameraFreeBridgeSystem.Setup(Instance);

        Log.Info = GD.Print;
        Log.Warn = GD.Print;
        Log.Error = GD.Print;
    }

    public override void _Process(double delta)
    {
        _deltaTimeEntity.Set(new SingletonDeltaTimeComponent { Value = (float)delta });
        Instance.Progress();
    }
}