using Godot;
using Flecs.NET.Core;
using Components.Animation;
using System.Collections.Generic;

namespace Systems.Bridge
{
    public static class AnimationTreeBridgeSystem
    {
        private static readonly Dictionary<Entity, string> LastAnimationStates = new();

        public static void Setup(World world)
        {
            world.System<AnimationComponent>()
                .Kind(Ecs.PostUpdate)
                .Each((Entity entity, ref AnimationComponent animComponent) =>
                {
                    if (!Kernel.NodeRef<Node3D>.TryGet(entity, out var node))
                        return;

                    string newState = animComponent.AnimationState ?? "";

                    // Only run when the animation state changes
                    if (LastAnimationStates.TryGetValue(entity, out var lastState) && lastState == newState)
                        return;

                    LastAnimationStates[entity] = newState;

                    foreach (Node n in node.GetTree().GetNodesInGroup("humanoid_animation_tree"))
                    {
                        if (n is AnimationTree animTree)
                        {
                            animTree.Active = true;

                            var playback = animTree.Get("parameters/StateMachine/playback")
                                                   .As<AnimationNodeStateMachinePlayback>();

                            if (playback != null)
                            {
                                string targetState = $"{newState}";
                                playback.Travel(targetState);
                                GD.Print($"[AnimationBridge] Entity {entity} -> Animation: {targetState}");
                            }
                            else
                            {
                                GD.PrintErr($"[AnimationBridge] Could not get StateMachine playback for entity {entity}");
                            }
                        }
                    }
                });
        }
    }
}
