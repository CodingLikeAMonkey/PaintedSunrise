using Godot;
using System;

namespace Kernel;

public partial class Utility : Node
{
    // public static string GetBaseLODPath(string scenePath)
    // {
    //     string fileName = scenePath.GetFile();
    //     string baseName = fileName.Split("--")[0];
    //     string directory = scenePath.GetBaseDir();
    //     return $"{directory}/LODs/{baseName}--LOD1.tscn";
    // }

    public static string GetUnifiedLOD1Path(string scenePath)
    {
        string fileName = scenePath.GetFile(); 
        string baseName = fileName.Split("--")[0];

        if (baseName.EndsWith(".tscn"))
            baseName = baseName.Substring(0, baseName.Length - ".tscn".Length);

        string directory = scenePath.GetBaseDir();

        return $"{directory}/LODs/{baseName}--LOD1.tscn";
    }



    public static string GetVariantLOD1Path(string scenePath)
    {
        return $"{scenePath}--LOD1.tscn";
    }

    // old version, expects a unique LOD1 per modifier

    // public static string GetBaseLODPath(string scenePath)
    // {
    //     string fileName = scenePath.GetFile();
    //     string baseName = fileName.Substring(0, fileName.LastIndexOf(".tscn"));
    //     string directory = scenePath.GetBaseDir();
    //     return $"{directory}/{baseName}--LOD1.tscn";
    // }
}
