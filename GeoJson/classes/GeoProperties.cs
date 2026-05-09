using Godot;
using Godot.Collections;

public partial class GeoProperties : Node
{
    // includes only properties I should care for
    private float minZoom = 0;

    public float GetMinZoom(){return minZoom;}

    public void ImportPropertyData(Dictionary<string,Variant> propertyData)
    {
        Variant minZoomVar;

        if (propertyData.TryGetValue("min_zoom",out minZoomVar))
            minZoom = (float)minZoomVar.AsDouble();
    }
}