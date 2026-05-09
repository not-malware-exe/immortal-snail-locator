using Godot;
using System;

public partial class ControllerAI : Node
{
	[Export]
	private float _speed = 100.0f;
	
	Marker marker = null;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		Node parent = GetParent();

		if (parent is Marker)
		{
			marker = parent as Marker;
		}

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		if (marker != null)
		{
			Vector2 input = Input.GetVector("leftward","rightward","backward","forward");

			marker.DisplaceWorldCoords(input * _speed * (float)delta);
		}
		
	}
}
