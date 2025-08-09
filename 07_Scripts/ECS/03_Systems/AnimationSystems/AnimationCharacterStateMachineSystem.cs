using Flecs.NET.Core;
using Components.Animation;
using Components.Character;
using Kernel;

namespace Systems.Animation;

public static class AnimationCharacterStateMachineSystem
{
    public static void Setup(World world)
    {
        world.System<CharacterStateComponent, AnimationComponent>()
            .Kind(Ecs.OnUpdate)
            .MultiThreaded()
            .Iter((Iter it, Field<CharacterStateComponent> cs, Field<AnimationComponent> ac) =>
            {
                for (int i = 0; i < it.Count(); i++)
                {
                    var characterState = cs[i];
                    ref var animationComponent = ref ac[i];

                    if (characterState.CurrentState == CharacterStateEnum.IdleState)
                        animationComponent.AnimationState = "humanoid_animation_library_idle";

                    else if (characterState.CurrentState == CharacterStateEnum.WalkState)
                        animationComponent.AnimationState = "humanoid_animation_library_walk";

                    else if (characterState.CurrentState == CharacterStateEnum.RunState)
                        animationComponent.AnimationState = "humanoid_animation_library_run";

                    else if (characterState.CurrentState == CharacterStateEnum.JumpState)
                        animationComponent.AnimationState = "humanoid_animation_library_jump";

                    else if (characterState.CurrentState == CharacterStateEnum.FallState)
                        animationComponent.AnimationState = "humanoid_animation_library_fall";

                    else if (characterState.CurrentState == CharacterStateEnum.LandingState)
                        animationComponent.AnimationState = "humanoid_animation_library_land";
                }

            });
    }
}