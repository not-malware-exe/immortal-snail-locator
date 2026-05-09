using Godot;

[GlobalClass]
public partial class CameraScale : Node
{
    private static Vector2 _cameraScale = new Vector2(1.0f,1.0f);

    public static Vector2 GetCameraScale(){return _cameraScale;}
    public static void SetCameraScale(Vector2 cameraScale){_cameraScale = cameraScale;}

}