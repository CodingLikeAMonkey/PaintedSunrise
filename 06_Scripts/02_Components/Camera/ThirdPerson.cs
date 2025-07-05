// Camera
namespace Components.Camera;

using Godot;

public struct ThirdPerson
{
    public Node3D Node;
    public float MinPitch;
    public float MaxPitch;

    public Vector3 rotDeg;
    public Vector2 lookvector;
    public float HorizontalMouseSensitivity;
    public float VerticalMouseSensitivity;
    public float HorizontalControllerSensitivity;
    public float VerticalControllerSensitivity;


    public ThirdPerson()
    {
        MinPitch = 35.0f;
        MaxPitch = -90.0f;
        HorizontalMouseSensitivity = 0.2f;
        VerticalMouseSensitivity = 0.2f;
        HorizontalControllerSensitivity = 2.0f;
        VerticalControllerSensitivity = 2.0f;

    }
    
}