using Flecs.NET.Core;
using Components.UI;
using Components.Input;
using Kernel;
using Components.Core;
using System.Data;

namespace Systems.UI;

public static class UIInteractiveSystem
{
    public static void Setup(World world, Entity inputEntity)
    {
        world.System<UIBoundingBoxComponent, UIInteractiveComponent, Transform2DComponent>()
            .Kind(Ecs.OnUpdate)
            .MultiThreaded()
            .Iter((Iter it, Field<UIBoundingBoxComponent> bb, Field<UIInteractiveComponent> inter, Field<Transform2DComponent> t) =>
            {
                var inputState = inputEntity.Get<InputStateComponent>();

                for (int i = 0; i < it.Count(); i++)
                {
                    Entity entity = it.Entity(i);
                    ref var boundingBox = ref bb[i];
                    ref var interactive = ref inter[i];
                    ref var transform = ref t[i];

                    bool wasHover = interactive.Hover;

                    bool nowHover =
                           inputState.MousePosition.X >= transform.Position.X &&
                           inputState.MousePosition.X <= transform.Position.X + boundingBox.Width &&
                           inputState.MousePosition.Y >= transform.Position.Y &&
                           inputState.MousePosition.Y <= transform.Position.Y + boundingBox.Height;

                    interactive.Hover = nowHover;

                    bool wasPressed = interactive.Click;
                    bool nowPressed = nowHover && inputState.LeftPressed;
                    interactive.Click = nowPressed;

                    if (!wasHover && nowHover)
                    {
                        entity.Set(new UIInteractionEventComponent
                        {
                            HoverEnter = true
                        });
                    }

                    else if (wasHover && !nowHover)
                    {
                        entity.Set(new UIInteractionEventComponent
                        {
                            HoverExit = true
                        });
                    }
                    if (inputState.LeftReleased)
                    {
                        if (nowHover)
                        {
                            if (wasPressed && !nowPressed && nowHover)
                            {
                                entity.Set(new UIInteractionEventComponent
                                {
                                    Click = true
                                });
                            }
                        }
                    }
                }
            });
    }
}