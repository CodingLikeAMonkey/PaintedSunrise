// Kernel/Services/DeltaTime.cs
using Flecs.NET.Core;
using Components.Core;

namespace Kernel.Services;

public static class DeltaTime
{
    private static Entity _entity;
    
    public static void Initialize(Entity entity)
    {
        if (entity.IsAlive())
        {
            _entity = entity;
            Log.PrintInfo("DeltaTime service initialized");
        }
        else
        {
            Log.PrintError("DeltaTime: Tried to initialize with invalid entity");
        }
    }
    
    public static float Value
    {
        get
        {
            if (_entity.IsAlive() && _entity.Has<Components.Core.DeltaTime>())
            {
                return _entity.Get<Components.Core.DeltaTime>().Value;
            }
            Log.PrintError("DeltaTime: Entity not alive or missing component");
            return 0f;
        }
    }
}