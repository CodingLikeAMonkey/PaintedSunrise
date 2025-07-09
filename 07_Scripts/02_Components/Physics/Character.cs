namespace Components.Physics;

public struct Character
{
    public bool IsOnFloor;
    public bool IsOnWall;
    public Components.Math.Vec3 FloorNormal;
    
    // For internal use by bridge system
    public bool GodotIsOnFloor;
    public Components.Math.Vec3 GodotFloorNormal;
}