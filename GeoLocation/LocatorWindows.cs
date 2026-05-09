#if GODOT_WINDOWS
using System;
using System.Threading.Tasks;
using Godot;
using Windows.Devices.Geolocation;

[GlobalClass]
public partial class LocatorWindows : Locator
{
	private Geolocator _geoLocator;
	// private bool _geoLocatorActive = false;
	private Vector2 _currentLocation = Vector2.Zero;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
        Task.Run(StartLocationUpdatesAsync);
	}


	public override Vector2 GetGPSCoords()
    {
        return _currentLocation;
    }

	private async Task StartLocationUpdatesAsync()
	{
	    // Always request access first
	    var accessStatus = await Geolocator.RequestAccessAsync().AsTask();

	    switch (accessStatus)
	    {
	        case GeolocationAccessStatus.Allowed:
	            _geoLocator = new Geolocator { 
					ReportInterval = 1000,
					DesiredAccuracy = PositionAccuracy.High,
					DesiredAccuracyInMeters = 10,
					MovementThreshold = 0
					};
				// _geoLocatorActive = true;
				_geoLocator.PositionChanged += OnPositionChanged;
	            break;

	        case GeolocationAccessStatus.Denied:
	            // App-specific: show explanation and guide user to settings
	            GD.PushWarning("Location access denied. Enable location in Settings > Privacy & Security > Location.");
	            break;

	        case GeolocationAccessStatus.Unspecified:
	            GD.PushWarning("Location access error occurred");
	            break;
	    }
	}
	
	private void OnPositionChanged(Geolocator sender, PositionChangedEventArgs args)
	{
	    var pos = args.Position.Coordinate.Point.Position;
	
	    _currentLocation = new Vector2((float)pos.Longitude, (float)pos.Latitude);
		GD.Print(_currentLocation);
	}

}
#endif