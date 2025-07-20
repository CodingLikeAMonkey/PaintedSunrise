namespace Components.Camera;

public struct ThirdPersonConfig
{
    public float MinPitch;
    public float MaxPitch;
    public float HorizontalMouseSensitivity;
    public float VerticalMouseSensitivity;
    public float HorizontalControllerSensitivity;
    public float VerticalControllerSensitivity;
    public int InvertVerticalControllerRotation; // should be either 1 or -1


    public ThirdPersonConfig()
    {
        MinPitch = 35.0f;
        MaxPitch = -90.0f;
        HorizontalMouseSensitivity = 0.2f;
        VerticalMouseSensitivity = 0.2f;
        HorizontalControllerSensitivity = 0.8f;
        VerticalControllerSensitivity = 0.8f;
        InvertVerticalControllerRotation = -1;
    }
}