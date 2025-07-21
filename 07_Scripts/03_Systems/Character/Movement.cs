using System.ComponentModel.Design.Serialization;
using Components.Core.Unique;
using Flecs.NET.Core;
using Kernel;

namespace Systems.Character
{
    public static class Movement
    {
        public static void Setup(World world, Entity inputEntity)
        {
            var camQuery = world.Query<Components.Camera.ThirdPersonConfig>();
            world.System<Components.Character.Player, Components.Physics.Velocity, Components.Character.MovementStats, Components.Character.Character, Components.Core.Transform>()
                .Kind(Ecs.OnUpdate)
                .MultiThreaded()
                .Iter((Iter it, Field<Components.Character.Player> p, Field<Components.Physics.Velocity> v, Field<Components.Character.MovementStats> s, Field<Components.Character.Character> c, Field<Components.Core.Transform> t) =>
                {
                    var inputState = inputEntity.Get<Components.Input.InputState>();
                    float delta = world.Entity("DeltaTime").Get<Components.Core.Unique.DeltaTime>().Value;
                    var cameraTransform = camQuery.First().Get<Components.Core.Transform>();

                    for (int i = 0; i < it.Count(); i++)
                    {
                        ref var velocity = ref v[i];
                        ref var character = ref c[i];
                        var stats = s[i];
                        ref var player = ref p[i];
                        ref var transform = ref t[i];

                        if (character.IsGrounded)
                        {
                            // last input detection
                            player.HasInput = inputState.LeftStickInputDir.Length() > 0.1f;

                            if (player.HasInput)
                            {
                                player.WalkInputHoldTime += delta;
                                player.LastInputDirection = inputState.LeftStickInputDir;
                                player.WasRotatingFromTap = true;
                            }
                            else
                            {
                                player.WalkInputHoldTime = 0.0f;
                            }

                            // cam direciton
                            Components.Math.Vec3 inputVector = new Components.Math.Vec3(player.LastInputDirection.X, 0, player.LastInputDirection.Y);
                            // Assuming your Quaternion has components: X, Y, Z, W

                            // Using your Quaternion methods properly
                            float yaw = cameraTransform.Rotation.ToEuler().Y;
                            Components.Math.Quaternion yawRotation = Components.Math.Quaternion.FromEuler(new Components.Math.Vec3(0, yaw, 0));



                            Components.Math.Vec3 cameraDirection = yawRotation.Rotate(inputVector).Normalized();



                            if (player.WalkInputHoldTime != 0.0f)
                            {
                                velocity.Value.X = cameraDirection.X * stats.Speed;
                                velocity.Value.Z = cameraDirection.Z * stats.Speed;
                                Log.Info(velocity.Value.ToString());
                            }
                            else
                            {
                                velocity.Value.X = 0.0f;
                                velocity.Value.Z = 0.0f;
                            }

                        }
                    }
                });
        }
    }
}