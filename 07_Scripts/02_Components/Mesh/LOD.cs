// Components/Mesh/LOD.cs
namespace Components.Mesh;

public struct LOD
{
    public int CurrentLod;
    public float CameraDistance;
    public string Lod1ScenePath;
    public string OriginalScenePath;
    public bool UseUnifiedLOD1;

    public LOD()
    {
        CurrentLod = 0;
        CameraDistance = 60.0f;
        Lod1ScenePath = "";
        OriginalScenePath = "";
        UseUnifiedLOD1 = true;
    }
}