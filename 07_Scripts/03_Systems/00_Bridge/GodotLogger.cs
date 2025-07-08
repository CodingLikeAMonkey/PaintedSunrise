using Godot;
using Kernel;

public partial class GodotLogger : Node
{
    public override void _Ready()
    {
        Log.Info = GD.Print;
        Log.Warn = GD.PushWarning;
        Log.Error = GD.PushError;
    }
}