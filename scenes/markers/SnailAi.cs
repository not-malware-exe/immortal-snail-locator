using Godot;
using System;

public partial class SnailAi : Node
{
	private Marker marker = null;

	[Export]
	private float speed = 1.0f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Node parent = GetParent();

		if (parent is Marker)
		{
			marker = parent as Marker;
		}
	}

	float time = 0.0f;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		time += (float)delta;
		if (time < 10.0f)return;

		Marker closestPlayerMarker = null;
		float closestPlayerMarkerDist = 99999999.9f;

		foreach (Node player in GetTree().GetNodesInGroup("Player"))
		{
			if (player is Marker)
			{
				Marker playerMarker = player as Marker;
				float playerMarkerDist = marker.GetDistanceInMetersToMarker(playerMarker);

				if (playerMarkerDist < closestPlayerMarkerDist)
				{
					closestPlayerMarker = playerMarker;
					closestPlayerMarkerDist = playerMarkerDist;
				}
			}
		}
		
		if (closestPlayerMarker != null)
		{
			marker.TravelToMarker(closestPlayerMarker,Math.Min(speed * (float)delta,closestPlayerMarkerDist));
		}
	}
}
