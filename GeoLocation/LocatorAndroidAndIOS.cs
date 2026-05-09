using System.Threading.Tasks;
using Godot;

[GlobalClass]
public partial class LocatorAndroidAndIOS : Locator
{
	Geolocation _geolocation = null;
	private Vector2 _currentLocation = Vector2.Zero;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (_geolocation is null)
		{
		    // Wrapper not registered as AutoLoad
		    return;
		}
		if (_geolocation.Supported)
		{
		    // Geolocation Plugin is supported

		    // Geolocation Settings
		    _geolocation.SetDebugLogSignal(true);
		    _geolocation.SetFailureTimeout(30);
		    // _geolocation.SetAutoCheckLocationCapability(true);

			startWatch();
		}
		else
		{
		    // Geolocation Plugin not supported
		}
	}

	Geolocation.LocationUpdater locationUpdater;

	private async Task startWatch()
	{
		// create and initialize updater
		locationUpdater = await _geolocation.GetLocationUpdater();

		if (locationUpdater is null)
		    {
		        var error = _geolocation.LastError;
		        return;
		    }

		Location location;
		while ((location = await locationUpdater.LocationUpdateAsync()) != null)
		{
		    _currentLocation = new Vector2((float)location.Longitude,(float)location.Latitude);
		}

		// we exited the loop ther because of Stop() or an error
		if(locationUpdater.HasError)
		{   
		    // get the error
		    var error =  locationUpdater.LastError;
		}
	}

	public override Vector2 GetGPSCoords()
    {
        return _currentLocation;
    }

	public override void _ExitTree()
	{
		if (locationUpdater != null)
    		locationUpdater.Stop();
	}
}
