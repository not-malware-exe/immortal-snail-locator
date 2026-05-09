using Godot;

[GlobalClass]
public partial class Locator : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	public virtual Vector2 GetGPSCoords()
    {
        return new Vector2(0.0f,0.0f);
    }
}
