// Entities/Physics/Character.cs
using Godot;
using Flecs.NET.Core;
using Components.Physics;
using Components.Core;

public partial class Character : CharacterBody3D
{
    private Entity _entity;

    public override void _Ready()
    {
        // Always register node
        _entity = Kernel.EcsWorld.Instance.Entity();
        Kernel.NodeRef.Register(_entity, this);
        
        // Add physics components
        _entity.Add<Gravity>()
               .Add<Character>()
               .Add<Transform>()
               .Add<Velocity>();
    }

    public override void _PhysicsProcess(double delta)
    {
        // Godot handles collision detection
        MoveAndSlide();
    }
}