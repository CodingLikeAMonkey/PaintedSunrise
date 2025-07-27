
using System.Collections.Generic;

namespace Components.Animation;

public struct AnimaitonClipComponent
{
    public string Name;
    public Dictionary<string, BoneTrack> BoneTrack;
    public float Length;
}