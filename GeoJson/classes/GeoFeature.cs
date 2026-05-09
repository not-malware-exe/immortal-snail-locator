
using Godot;
using Godot.Collections;

public partial class GeoFeature : Node
{
    private GeoGeometry geoGeometry = null;
    private string geoType = "";

    public GeoGeometry GetGeoGeometry(){return geoGeometry;}
    public string GetGeoType(){return geoType;}


    private GeoProperties geoProperties = null;
    
    public GeoProperties GetGeoProperties(){return geoProperties;}


    public bool importFeatureData(Dictionary<string,Variant> featureData)
    {
        (string geoType, Array<Variant> coordsData) = ReadGeometry(featureData);
        SetGeometryData(geoType,coordsData);
        
        SetPropertyData(featureData);
        
        return true;
    }

    public static (string, Array<Variant>) ReadGeometry(Dictionary<string,Variant> featureData)
    {
        Variant geometryVar;

        if (featureData.TryGetValue("geometry",out geometryVar))
        {
            Dictionary<string,Variant> geometryData = geometryVar.AsGodotDictionary<string,Variant>();
            Variant geoTypeVar;
            Variant coordsVar;

            if (geometryData.TryGetValue("type", out geoTypeVar))
            {
                if (geometryData.TryGetValue("coordinates", out coordsVar))
                {
                    return (geoTypeVar.AsString(), coordsVar.AsGodotArray<Variant>());
                }
            }
        }

        return ("",[]);
    }

    private bool SetGeometryData(string geoType, Array<Variant> coordsData)
    {
        switch (geoType)
        {
            case "Polygon":
                geoGeometry = new GeoGeometryPolygon();
                geoGeometry.ImportCoordsData(coordsData);
                break;
            case "MultiPolygon":
                geoGeometry = new GeoGeometryMultiPolygon();
                geoGeometry.ImportCoordsData(coordsData);
                break;
            case "LineString":
                geoGeometry = new GeoGeometryLine();
                geoGeometry.ImportCoordsData(coordsData);
                break;
            case "MultiLineString":
                geoGeometry = new GeoGeometryMultiLine();
                geoGeometry.ImportCoordsData(coordsData);
                break;
            default:
                return false;
        }
        return true;
    }

    private void SetPropertyData(Dictionary<string,Variant> featureData)
    {
        Variant propertyDataVar;

        if (featureData.TryGetValue("properties",out propertyDataVar))
        {
            Dictionary<string,Variant> propertyData = propertyDataVar.AsGodotDictionary<string,Variant>();
            geoProperties = new GeoProperties();
            geoProperties.ImportPropertyData(propertyData);
        }
    }
}