using System;
using System.Collections.Generic;
using System.Xml;
using Flecs.NET.Core;
using Components.XML;
using Kernel;

namespace Systems.XML
{
    public static class XMLParserSystem
    {
        // Cache: filepath -> (last modified time, parsed root XmlNode)
        private static Dictionary<string, (DateTime lastModified, XmlNode rootNode)> _cache = new();

        public static void Setup(World world)
        {
            world.System<XMLFileComponent>()
                .Kind(Ecs.OnUpdate)
                .MultiThreaded()
                .Iter((Iter it, Field<XMLFileComponent> f) =>
                {
                    for (int i = 0; i < it.Count(); i++)
                    {
                        var file = f[i];
                        if (!System.IO.File.Exists(file.FilePath))
                        {
                            Log.Warn($"File not found: {file.FilePath}");
                            continue;
                        }

                        DateTime lastWriteTime = System.IO.File.GetLastWriteTime(file.FilePath);

                        if (_cache.TryGetValue(file.FilePath, out var cached))
                        {
                            if (cached.lastModified == lastWriteTime)
                            {
                                // File unchanged, reuse cached data, or just skip parsing
                                Log.Info($"No changes detected in {file.FilePath}, skipping parse.");
                                continue;
                            }
                        }

                        try
                        {
                            var xmlDoc = new XmlDocument();
                            xmlDoc.Load(file.FilePath);

                            // Cache the root node and timestamp
                            _cache[file.FilePath] = (lastWriteTime, xmlDoc.DocumentElement);

                            Log.Info($"Parsing updated XML file: {file.FilePath}");
                            PrintXmlNode(xmlDoc.DocumentElement);
                        }
                        catch (XmlException ex)
                        {
                            Log.Error($"XML parse error: {ex.Message}");
                        }
                    }
                });
        }

        private static void PrintXmlNode(XmlNode node, int depth = 0)
        {
            if (node == null) return;

            string indent = new string(' ', depth * 2);
            Log.Info($"{indent}Tag: <{node.Name}>");

            if (node.Attributes != null)
            {
                foreach (XmlAttribute attr in node.Attributes)
                {
                    Log.Info($"{indent}  Attribute: {attr.Name} = \"{attr.Value}\"");
                }
            }

            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.NodeType == XmlNodeType.Element)
                {
                    PrintXmlNode(child, depth + 1);
                }
            }
        }
    }
}
