// Systems/Bridge/PhysicsBridge.cs
using Godot;
using Flecs.NET.Core;
using Components.Physics;
using Components.Core;
using Kernel;

namespace Systems.Bridge;

public static class PhysicsBridge
{
    public static void Setup(World world)
    {
        // Write ECS state to Godot before physics step
        world.System()
            .Kind(Ecs.PreUpdate)
            .With<Transform, Velocity>().Write()
            .Iter((Iter it) =>
            {
                foreach (int i in it)
                {
                    Entity e = it.Entity(i);
                    if (!NodeRef.TryGet(e, out Node3D node)) continue;

                    ref Transform t = ref it.Field<Transform>(1)[i];
                    ref Velocity v = ref it.Field<Velocity>(2)[i];

                    // Apply transform and velocity
                    node.GlobalPosition = t.Position.ToGodot();
                    node.GlobalRotation = t.Rotation.ToGodot();
                    
                    if (node is CharacterBody3D character)
                    {
                        character.Velocity = v.Linear.ToGodot();
                    }
                    else if (node is RigidBody3D rigid)
                    {
                        rigid.LinearVelocity = v.Linear.ToGodot();
                        rigid.AngularVelocity = v.Angular.ToGodot();
                    }
                }
            });

        // Read Godot state to ECS after physics step
        world.System()
            .Kind(Ecs.PostUpdate)
            .With<Transform, Velocity>().Read()
            .Iter((Iter it) =>
            {
                foreach (int i in it)
                {
                    Entity e = it.Entity(i);
                    if (!NodeRef.TryGet(e, out Node3D node)) continue;

                    ref Transform t = ref it.Field<Transform>(1)[i];
                    ref Velocity v = ref it.Field<Velocity>(2)[i];

                    // Update transform and velocity
                    t.Position = (Components.Math.Vec3)node.GlobalPosition;
                    t.Rotation = (Components.Math.Vec3)node.GlobalRotation;
                    
                    if (node is CharacterBody3D character)
                    {
                        v.Linear = (Components.Math.Vec3)character.Velocity;
                        character.GetFloorNormal();
                    }
                    else if (node is RigidBody3D rigid)
                    {
                        v.Linear = (Components.Math.Vec3)rigid.LinearVelocity;
                        v.Angular = (Components.Math.Vec3)rigid.AngularVelocity;
                    }
                }
            });
    }
}