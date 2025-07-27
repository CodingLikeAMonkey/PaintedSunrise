using System.Collections.Generic;
using Components.Math;

namespace Components.Animation;

public struct BoneTrack
{
    public List<Vec3Component> Positions;
    public List<QuaternionComponent> Rotation;
    public List<Vec3Component> Scales;
    public List<float> Timestamps;
}