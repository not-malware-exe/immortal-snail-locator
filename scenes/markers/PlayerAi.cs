using Godot;
using System;

public partial class PlayerAi : Node
{
	Locator locator = null;
	Marker marker = null;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Node parent = GetParent();

		if (parent is Marker)
		{
			marker = parent as Marker;
		}

#if GODOT_WINDOWS
		locator = new LocatorWindows();
#endif
#if GODOT_ANDROID || GODOT_IPHONE
		GD.PushWarning("Code for android and ios missing.");
#endif

		if (locator != null)
			AddChild(locator);

		GD.Print(marker,locator);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		if (locator != null && marker != null)
		{
			marker.SetWorldCoords(locator.GetGPSCoords());
		}
		
	}
}
