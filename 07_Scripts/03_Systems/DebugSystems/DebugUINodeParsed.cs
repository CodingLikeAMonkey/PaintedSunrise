using Flecs.NET.Core;
using Components.UI;
using Classes.UI;
using Kernel;

namespace Systems.Debug;

public static class DebugUINodeParsed
{
    public static void Setup(World world)
    {
        world.System<ParsedUIComponent>()
            .Kind(Ecs.OnUpdate)
            .MultiThreaded()
            .Iter((Iter it, Field<ParsedUIComponent> p) =>
            {
                for (int i = 0; i < it.Count(); i++)
                {
                    ref var parsed = ref p[i];
                    
                    Log.Info("Printing parsed UI tree:");
                    PrintUINode(parsed.RootNode);

                }
            });
    }

    private static void PrintUINode(UINode node, int depth = 0)
    {
        if (node == null) return;
        string indent = new string(' ', depth * 2);
        Log.Info($"{indent}Node: {node.Name}, Type: {node.Type}");
        foreach (var attr in node.Attributes)
        {
            Log.Info($"{indent}  Attribute: {attr.Key} = {attr.Value}");
        }
        if (!string.IsNullOrEmpty(node.TextContent))
        {
            Log.Info($"{indent}  TextContent: {node.TextContent}");
        }
        foreach (var child in node.Children)
        {
            PrintUINode(child, depth + 1);
        }
    }
}