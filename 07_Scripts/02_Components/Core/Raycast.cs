#nullable enable
using Flecs.NET.Core;
using Godot;

namespace Components.Core;

public struct Raycast
{
    public Node3D Node;
    public Vector3 Direction;
    public float Length;
    public uint CollisionMask;
    public Vector3 Offset;

    public bool Hit;          // current frame hit
    public bool PreviousHit;  // last frame hit

    public Vector3 HitPosition;
    public Vector3 HitNormal;
    public Node3D? HitNode;
    public bool DebugDraw;
    public Entity HitEntity;
}
