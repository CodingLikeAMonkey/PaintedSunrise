using Godot;
using Kernel;

namespace Systems.Bridge;
public partial class GodotLogger : Node
{
    public override void _Ready()
    {
        Log.Info = GD.Print;
        Log.Warn = GD.PushWarning;
        Log.Error = GD.PushError;
    }
}