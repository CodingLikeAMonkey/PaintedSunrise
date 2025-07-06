// Systems/Shaders/OutlineHelper.cs
using Godot;
using Flecs.NET.Core;
using Components.Shaders;

namespace Systems.Shaders;

public static class OutlineHelper
{
    public static void EnsureMaterialUnique(Entity entity, MeshInstance3D meshInstance)
    {
        // Skip if already processed
        if (entity.Has<OutlineMaterialProcessed>())
            return;

        bool madeUnique = false;

        // Process material override
        if (meshInstance.MaterialOverride != null && IsOutlineMaterial(meshInstance.MaterialOverride))
        {
            meshInstance.MaterialOverride = meshInstance.MaterialOverride.Duplicate() as Material;
            madeUnique = true;
        }

        // Process surface materials
        if (meshInstance.Mesh != null)
        {
            int surfaceCount = meshInstance.Mesh.GetSurfaceCount();
            for (int s = 0; s < surfaceCount; s++)
            {
                Material mat = meshInstance.GetSurfaceOverrideMaterial(s) ?? 
                              meshInstance.Mesh.SurfaceGetMaterial(s);
                
                if (mat != null && IsOutlineMaterial(mat))
                {
                    meshInstance.SetSurfaceOverrideMaterial(s, mat.Duplicate() as Material);
                    madeUnique = true;
                }
            }
        }

        // Mark entity as processed
        if (madeUnique)
            entity.Add<OutlineMaterialProcessed>();
    }

    public static bool IsOutlineMaterial(Material material)
    {
        Material current = material;
        while (current != null)
        {
            if (current is ShaderMaterial shaderMat &&
                shaderMat.Shader != null &&
                shaderMat.Shader.ResourcePath.EndsWith("outline.gdshader"))
            {
                return true;
            }
            current = current.NextPass;
        }
        return false;
    }

    public static void SetOutlineStrength(MeshInstance3D meshInstance, float strength)
    {
        // Material override
        if (meshInstance.MaterialOverride is ShaderMaterial overrideMat &&
            IsOutlineMaterial(overrideMat))
        {
            overrideMat.SetShaderParameter("outline_strength", strength);
        }

        // Surface materials
        if (meshInstance.Mesh != null)
        {
            int surfaceCount = meshInstance.Mesh.GetSurfaceCount();
            for (int s = 0; s < surfaceCount; s++)
            {
                Material mat = meshInstance.GetSurfaceOverrideMaterial(s) ?? 
                              meshInstance.Mesh.SurfaceGetMaterial(s);
                
                if (mat is ShaderMaterial shaderMat && IsOutlineMaterial(shaderMat))
                {
                    shaderMat.SetShaderParameter("outline_strength", strength);
                }
            }
        }
    }
}