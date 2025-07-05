using Flecs.NET.Core;
using Godot;
using System;

namespace Systems.Shaders;

public static class ClearOutline
{
    public static void Setup(World world)
    {
        world.System<Components.Mesh.Selectable, Components.Core.Transform>()
            .Without<Components.Shaders.Outline>()
            .Kind(Ecs.OnUpdate)
            .Iter((Iter it, Field<Components.Mesh.Selectable> selectableMesh, Field<Components.Core.Transform> transforms) =>
            {
                for (int i = 0; i < it.Count(); i++)
                {
                    var meshData = selectableMesh[i];

                    if (meshData.Node == null)
                        continue;

                    foreach (Node child in meshData.Node.GetChildren())
                    {
                        if (child is MeshInstance3D meshInstance)
                        {
                            bool hasOutlineShader = false;

                            // Check material override
                            if (meshInstance.MaterialOverride != null)
                            {
                                hasOutlineShader = ResetOutlineIfOutlineMaterial(meshInstance.MaterialOverride);
                            }

                            // Check surface materials
                            if (meshInstance.Mesh != null && !hasOutlineShader)
                            {
                                int surfaceCount = meshInstance.Mesh.GetSurfaceCount();
                                for (int s = 0; s < surfaceCount; s++)
                                {
                                    Material mat = meshInstance.GetSurfaceOverrideMaterial(s)
                                                 ?? meshInstance.Mesh.SurfaceGetMaterial(s);

                                    if (mat != null && ResetOutlineIfOutlineMaterial(mat))
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            });
    }

    private static bool ResetOutlineIfOutlineMaterial(Material material)
    {
        bool foundOutline = false;
        Material current = material;

        while (current != null)
        {
            if (current is ShaderMaterial shaderMat &&
                shaderMat.Shader != null &&
                shaderMat.Shader.ResourcePath.EndsWith("outline.gdshader"))
            {
                shaderMat.SetShaderParameter("outline_strength", 0);
                foundOutline = true;
            }

            current = current.NextPass;
        }

        return foundOutline;
    }
}
