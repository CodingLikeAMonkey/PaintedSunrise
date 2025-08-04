using Flecs.NET.Core;
using Godot;
using Components.Singleton;
using Components.Input;
using Systems.Core;
using Systems.Debug;
using Systems.Character;
using Systems.Input;
using Components.UI;
using Systems.UI;
using Components.XML;
using Systems.XML;
using Systems.Time;
using Ssytems.Lighting;
using Systems.Bridge;


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
        Instance.Component<InputStateComponent>();
        Instance.Component<UIBoundingBoxComponent>();
        Instance.Component<UIInteractionEventComponent>();
        Instance.Component<UIInteractiveComponent>();
        Instance.Component<XMLFileComponent>();
        Instance.Component<SingletonDayTimeComponent>();


        InputEntity = Instance.Entity("Singleton")
            .Set(new SingletonGameStateComponent())
            .Set(new SingletonMouseModeComponent())
            .Set(new InputStateComponent());

        Entity dayTimeEntity = Instance.Entity("DayTime")
            .Set(new SingletonDayTimeComponent
            {
                Day = 1,
                Hour = 0.0f,
                TimeScale = 1f
            });

        _deltaTimeEntity = Instance.Entity("DeltaTime")
            .Set(new SingletonDeltaTimeComponent { Value = 0f });


        Entity mainMenuXML = Instance.Entity()
            .Set(new XMLFileComponent
            {
                FilePath = Kernel.Utility.GetPath("07_Scripts/XML/MainMenu.xml")
            })
            .Add<ParsedUIComponent>();

        MouseModeSystem.Setup(Instance);
        InputCameraThirdPersonSystem.Setup(Instance, InputEntity);
        Systems.Input.InputCameraFreeSystem.Setup(Instance, InputEntity);
        Systems.Camera.CameraFreeSystem.Setup(Instance, InputEntity);
        Systems.Core.Fsm.GameStateSystem.Setup(Instance, InputEntity);
        CharacterMovementSystem.Setup(Instance, InputEntity);
        Systems.Camera.CameraThirdPersonSystem.Setup(Instance);
        CharacterStateSystem.Setup(Instance);
        CharacterJumpSystem.Setup(Instance, InputEntity);
        CharacterLastPositonSystem.Setup(Instance);
        UIInteractiveSystem.Setup(Instance, InputEntity);
        DebugUIInteractiveStats.Setup(Instance);
        XMLParserSystem.Setup(Instance);
        DayTimeSystem.Setup(Instance);
        LightingDayNightSystem.Setup(Instance, dayTimeEntity);
        // DebugDayTime.Setup(Instance);
        // DebugUINodeParsed.Setup(Instance);
        // DebugGameStateSystem.Setup(Instance);

        // DebugPrintCharacterStateSystem.Setup(Instance);


        // Bridge Systems
        // MeshLODBridgeSystem.Setup(Instance);
        CameraThirdPersonBridgeSystem.Setup(Instance);
        CharacterPhysicsBridgeSystem.Setup(Instance);
        CameraSetCurrentBridgeSystem.Setup(Instance);
        LightingDayNightBridgeSystem.Setup(Instance);

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