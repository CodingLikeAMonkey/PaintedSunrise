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
    public bool UseUnifiedLOD1;
    public string OriginalScenePath;
    public PackedScene OriginalPacked;

    public LOD()
    {
        CurrentLod = 0;
        VariantName = "";
        CameraDistance = 60.0f;
        Lod1ScenePath = "";
        Lod1Packed = null;
        OriginalScenePath = "";
        OriginalPacked = null;
        UseUnifiedLOD1 = true;
    }
}
