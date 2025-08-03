using Godot;
using Flecs.NET.Core;
using Components.Character;

[GlobalClass]
public partial class CRS_CharacterMovementStats : ComponentResource
{
    [Export] public float WalkThreshold = 0.7f;
    [Export] public float TapThreshold = 0.06f;
    [Export] public float Speed = 3.0f;
    [Export] public float WalkSpeed = 1.2f;
    [Export] public float Acceleration = 25.0f;
    [Export] public float Friction = 35.0f;
    [Export] public float TurnSpeed = 10.0f;
    [Export] public float JumpImpulse = 5.0f;
    [Export] public float AirAcceleration = 0.05f;
    [Export] public float AirTurnSpeed = 0.1f;

    public override void ApplyToEntity(Entity entity, Node3D ownerNode)
    {
        var data = new CharacterMovementStatsComponent
        {
            WalkThreshold = WalkThreshold,
            TapThreshold = TapThreshold,
            Speed = Speed,
            WalkSpeed = WalkSpeed,
            Acceleration = Acceleration,
            Friction = Friction,
            TurnSpeed = TurnSpeed,
            JumpImpulse = JumpImpulse,
            AirAcceleration = AirAcceleration,
            AirTurnSpeed = AirTurnSpeed
        };

        entity.Set(data);
    }
}
