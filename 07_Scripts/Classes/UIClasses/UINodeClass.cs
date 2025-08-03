using System.Collections.Generic;

namespace Classes.UI;

public class UINode
{
    public string Name;
    public string Type;
    public Dictionary<string, string> Attributes = new();
    public List<UINode> Children = new();
    public string TextContent = null;
}