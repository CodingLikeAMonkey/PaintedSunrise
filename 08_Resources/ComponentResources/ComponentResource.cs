using Godot;
using Flecs.NET.Core;
using System;

[GlobalClass]
public abstract partial class ComponentResource : Resource
{
    public abstract void ApplyToEntity(Entity entity);
}
