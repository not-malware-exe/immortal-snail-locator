using Godot;
using System;

[GlobalClass]
public partial class Marker : Node2D
{
	private Vector2 worldCoords = new Vector2(0,0);

	public void DisplaceWorldCoords(Vector2 displacement)
	{
		SetWorldCoords(worldCoords + displacement);
	}

	public void SetWorldCoords(Vector2 newWorldCoords)
	{
		worldCoords = newWorldCoords;

		if (worldCoords.X > 180) worldCoords.X -= 360;
		else if (worldCoords.X < -180) worldCoords.X += 360;
		if (worldCoords.Y > 90) worldCoords.Y = 90;
		else if (worldCoords.Y < -90) worldCoords.Y = -90;
	}

	public Vector2 GetWorldCoords() {return worldCoords;}


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		worldCoords = Position * new Vector2(1.0f,-1.0f);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position = worldCoords * new Vector2(1.0f,-1.0f);
	}

	public static float DegToRad(float degrees)
	{
		return degrees / 180.0f * (float)Math.PI;
	}
	public static double DegToRad(double degrees)
	{
		return degrees / 180.0f * Math.PI;
	}
	public static float RadToDeg(float radians)
	{
		return radians / (float)Math.PI * 180.0f;
	}
	public static double RadToDeg(double radians)
	{
		return radians / Math.PI * 180.0f;
	}
	public static float ConvertRadiansToEarthSurfaceMeters(float radians)
	{
		float radiusOfEarth = 6378137.0f;
		float meters = radians * radiusOfEarth;
		
		return meters;
	}

	public float GetDistanceInMetersToMarker(Marker m)
	{
		float distRadians = GetDistanceInRadiansToMarker(m);
		float distMeters = ConvertRadiansToEarthSurfaceMeters(distRadians);
		
		return distMeters;
	}


	public float GetDistanceInRadiansToMarker(Marker m)
	{
		Vector2 coords1 = worldCoords;
		Vector2 coords2 = m.GetWorldCoords();

		double lat1 = coords1.Y;
		double lat2 = coords2.Y;
		double lon1 = coords1.X;
		double lon2 = coords2.X;

		double latRad1 = DegToRad(lat1);
		double latRad2 = DegToRad(lat2);
		double deltaLatRad = DegToRad(lat2 - lat1);
		double deltaLonRad = DegToRad(lon2 - lon1);

		double angle = (
					Math.Pow(Math.Sin(deltaLatRad/2.0),2.0) +
    				Math.Cos(latRad1) * Math.Cos(latRad2) *
          			Math.Pow(Math.Sin(deltaLonRad/2.0),2.0)
						);

		double distRadians = 2.0f * Math.Atan2(Math.Sqrt(angle),Math.Sqrt(1.0f-angle));
		
		return (float)distRadians;
	}

	public static double GetBearing(double lat1, double lon1, double lat2, double lon2)
	{
    	double latRad1 = DegToRad(lat1);
    	double latRad2 = DegToRad(lat2);
    	double deltaLonRad = DegToRad(lon2 - lon1);

    	double y = Math.Sin(deltaLonRad) * Math.Cos(latRad2);
    	double x = Math.Cos(latRad1) * Math.Sin(latRad2) -
               Math.Sin(latRad1) * Math.Cos(latRad2) * Math.Cos(deltaLonRad);

    	return Math.Atan2(y, x);
	}

	public void TravelToMarker(Marker m, float distMeters)
	{
		float radiusOfEarth = 6378137.0f;

		Vector2 coords1 = worldCoords;
		Vector2 coords2 = m.GetWorldCoords();

		double lat1 = coords1.Y;
		double lat2 = coords2.Y;
		double lon1 = coords1.X;
		double lon2 = coords2.X;

		double bearingRad = GetBearing(lat1, lon1, lat2, lon2);

		double latRad = DegToRad(lat1);
    	double lonRad = DegToRad(lon1);
    	double angularDist = distMeters / radiusOfEarth;

    	double destLatRad = Math.Asin(
							Math.Sin(latRad) * Math.Cos(angularDist) +
                            Math.Cos(latRad) * Math.Sin(angularDist) * Math.Cos(bearingRad)
								);

    	double destLonRad = lonRad + Math.Atan2(
									Math.Sin(bearingRad) * Math.Sin(angularDist) * Math.Cos(latRad),
                                    Math.Cos(angularDist) - Math.Sin(latRad) * Math.Sin(destLatRad)
									);

		double destLat = RadToDeg(destLatRad);
		double destLon = RadToDeg(destLonRad);

		Vector2 coords = new Vector2((float)destLon,(float)destLat);

		SetWorldCoords(coords);
	}

}
