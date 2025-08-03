using Godot;
using Flecs.NET.Core;
using Components.Character;

[GlobalClass]
public partial class CRS_CharacterMovementStats : ComponentResource
{
    [Export] private float WalkThreshold = 0.7f;
    [Export] private float TapThreshold = 0.06f;
    [Export] private float Speed = 3.0f;
    [Export] private float WalkSpeed = 1.2f;
    [Export] private float Acceleration = 25.0f;
    [Export] private float Friction = 35.0f;
    [Export] private float TurnSpeed = 10.0f;
    [Export] private float JumpImpulse = 5.0f;
    [Export] private float AirAcceleration = 0.05f;
    [Export] private float AirTurnSpeed = 0.1f;

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
