using Godot;
using System;

namespace Components.Mesh;

public struct LOD
{
    public int CurrentLod;
    public string VariantName;
    public float CameraDistance;
    public string Lod1ScenePath;
    public PackedScene Lod1Packed;

    public LOD()
    {
        CurrentLod = 0;
        VariantName = "";
        CameraDistance = 300.0f;
        Lod1ScenePath = "";
        Lod1Packed = null;
    }
}
