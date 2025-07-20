// Camera
namespace Components.Camera;


public struct ThirdPersonConfig
{
    public float MinPitch;
    public float MaxPitch;
    public float HorizontalMouseSensitivity;
    public float VerticalMouseSensitivity;
    public float HorizontalControllerSensitivity;
    public float VerticalControllerSensitivity;


    public ThirdPersonConfig()
    {
        MinPitch = 35.0f;
        MaxPitch = -90.0f;
        HorizontalMouseSensitivity = 0.2f;
        VerticalMouseSensitivity = 0.2f;
        HorizontalControllerSensitivity = 2.0f;
        VerticalControllerSensitivity = 2.0f;
    }
}