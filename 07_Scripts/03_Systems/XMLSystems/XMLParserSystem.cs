using System;
using System.Collections.Generic;
using System.Xml;
using Flecs.NET.Core;
using Components.XML;
using Components.UI;
using Classes.UI;
using Kernel;

namespace Systems.XML
{
    public static class XMLParserSystem
    {
        // Cache storing last modified time and root XML node
        private static Dictionary<string, (DateTime lastModified, XmlNode rootNode)> _cache = new();

        // Track when we last logged "no change" per file to avoid spamming logs
        private static Dictionary<string, DateTime> _lastLoggedNoChange = new();

        public static void Setup(World world)
        {
            world.System<XMLFileComponent, ParsedUIComponent>()
                .Kind(Ecs.OnUpdate)
                .MultiThreaded()
                .Iter((Iter it, Field<XMLFileComponent> f, Field<ParsedUIComponent> p) =>
                {
                    for (int i = 0; i < it.Count(); i++)
                    {
                        var file = f[i];
                        ref var parsedUI = ref p[i];

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
                                // Only log once per unchanged timestamp
                                if (!_lastLoggedNoChange.TryGetValue(file.FilePath, out var loggedTime) || loggedTime != lastWriteTime)
                                {
                                    Log.Info($"No changes detected in {file.FilePath}, skipping parse.");
                                    _lastLoggedNoChange[file.FilePath] = lastWriteTime;
                                }
                                continue;
                            }
                            else
                            {
                                // File changed - clear logged no-change record to allow future no-change logs
                                _lastLoggedNoChange[file.FilePath] = DateTime.MinValue;
                            }
                        }

                        try
                        {
                            var xmlDoc = new XmlDocument();
                            xmlDoc.Load(file.FilePath);
                            _cache[file.FilePath] = (lastWriteTime, xmlDoc.DocumentElement);

                            UINode rootUINode = ConvertXmlNodeToUINode(xmlDoc.DocumentElement);

                            // Update the parsed UI component directly
                            parsedUI = new ParsedUIComponent { RootNode = rootUINode };

                            Log.Info($"Parsed and updated UI tree from {file.FilePath}");
                        }
                        catch (XmlException ex)
                        {
                            Log.Error($"XML parse error in {file.FilePath}: {ex.Message}");
                        }
                    }
                });
        }

        private static UINode ConvertXmlNodeToUINode(XmlNode xmlNode)
        {
            if (xmlNode == null)
                return null;

            var uiNode = new UINode
            {
                Name = xmlNode.Name,
                Type = xmlNode.Attributes?["is-type"]?.Value ?? "control",
                TextContent = xmlNode.InnerText.Trim().Length > 0 ? xmlNode.InnerText.Trim() : null,
                Attributes = new Dictionary<string, string>()
            };

            if (xmlNode.Attributes != null)
            {
                foreach (XmlAttribute attr in xmlNode.Attributes)
                {
                    uiNode.Attributes[attr.Name] = attr.Value;
                }
            }

            foreach (XmlNode child in xmlNode.ChildNodes)
            {
                if (child.NodeType == XmlNodeType.Element)
                {
                    var childUINode = ConvertXmlNodeToUINode(child);
                    if (childUINode != null)
                        uiNode.Children.Add(childUINode);
                }
            }

            return uiNode;
        }
    }
}
