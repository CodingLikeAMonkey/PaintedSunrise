using Godot;
using Flecs.NET.Core;

namespace Systems.Shaders;

public static class Outline
{
    public static void Setup(World world)
    {
        world.System<Components.Mesh.Selectable, Components.Core.Transform, Components.Shaders.Outline>()
            .Kind(Ecs.OnUpdate)
            .Iter((Iter it, Field<Components.Mesh.Selectable> selectableMesh, Field<Components.Core.Transform> transforms, Field<Components.Shaders.Outline> outlines) =>
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
                            string location = "";

                            // Check material override
                            if (meshInstance.MaterialOverride != null)
                            {
                                hasOutlineShader = SetOutlineStrengthIfOutlineMaterial(meshInstance.MaterialOverride);
                                if (hasOutlineShader)
                                    location = "MaterialOverride";
                            }

                            // Check surface materials
                            if (!hasOutlineShader && meshInstance.Mesh != null)
                            {
                                int surfaceCount = meshInstance.Mesh.GetSurfaceCount();
                                for (int s = 0; s < surfaceCount; s++)
                                {
                                    Material mat = meshInstance.GetSurfaceOverrideMaterial(s)
                                                 ?? meshInstance.Mesh.SurfaceGetMaterial(s);

                                    if (mat != null && SetOutlineStrengthIfOutlineMaterial(mat))
                                    {
                                        hasOutlineShader = true;
                                        location = $"Surface {s}";
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            });
    }

    private static bool SetOutlineStrengthIfOutlineMaterial(Material material)
    {
        bool foundOutline = false;
        Material current = material;

        while (current != null)
        {
            if (current is ShaderMaterial shaderMat &&
                shaderMat.Shader != null &&
                shaderMat.Shader.ResourcePath.EndsWith("outline.gdshader"))
            {
                shaderMat.SetShaderParameter("outline_strength", 1);
                foundOutline = true;
            }

            current = current.NextPass;
        }

        return foundOutline;
    }
}
