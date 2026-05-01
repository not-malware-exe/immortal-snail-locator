
using System.Linq;
using Godot;
using Godot.Collections;
using Windows.Foundation.Metadata;

[GlobalClass]
public partial class GeoJsonLoader : Node
{
    public override void _Ready()
    {
        base._Ready();
        
        float t1 = Time.GetTicksMsec();

        Dictionary<string,Variant> landJson = loadJson("res://GeoJson/ne_50m_land.json");

        GD.Print("Load Json: ", Time.GetTicksMsec()-t1);
        t1 = Time.GetTicksMsec();

        Array<Dictionary<string,Variant>> landFeatures = GetFeatures(landJson);

        Array<Vector2> polygon = [];
        Array polygons = [];

        foreach (Dictionary<string,Variant> landFeature in landFeatures)
        {
            Dictionary<string,Variant> landGeometry = GetGeometry(landFeature);
            (string landGeoType, Array<Variant> landCoordsData) = ReadGeometry(landGeometry);

            if (landGeoType != "Polygon") continue;

            Array<Vector2> landCoords = GetPolygonCoords(landCoordsData);

            Array<int> polygon_points = [];
            for (int i = 0; i < landCoords.Count; i++) polygon_points.Add(i + polygon.Count);

            polygon.AddRange(landCoords);
            polygons.Add(polygon_points.ToArray());
        }

        GD.Print("Getting All Coords: ", Time.GetTicksMsec()-t1);
        t1 = Time.GetTicksMsec();

        Polygon2D newPolygon2D = GetNode<Polygon2D>("Polygon2D");
        newPolygon2D.Polygon = polygon.ToArray();
        newPolygon2D.Polygons = polygons;

        GD.Print("Setting Polygon2D: ", Time.GetTicksMsec()-t1);

        GD.Print("Bytes: ", GD.VarToBytes(polygon.ToArray()).Count() + GD.VarToBytes(polygons).Count());
    }

    public override void _Process(double delta)
    {
        GD.Print(delta);
    }

    public static Dictionary<string,Variant> loadJson(string path)
	{
		FileAccess file = FileAccess.Open(path, FileAccess.ModeFlags.Read);
		string jsonText = file.GetAsText();

		Dictionary<string,Variant> json = Json.ParseString(jsonText).AsGodotDictionary<string,Variant>();

        return json;
	}

    public static Array<Dictionary<string,Variant>> GetFeatures(Dictionary<string,Variant> GeoJson)
    {
        Variant featuresVar;
        
        if (GeoJson.TryGetValue("features",out featuresVar))
        {
            return featuresVar.AsGodotArray<Dictionary<string,Variant>>();
        }

        return [];
    }

    public static Dictionary<string,Variant> GetGeometry(Dictionary<string,Variant> feature)
    {
        Variant geometryVar;

        if (feature.TryGetValue("geometry",out geometryVar))
        {
            return geometryVar.AsGodotDictionary<string,Variant>();
        }

        return [];
    }

    public static (string, Array<Variant>) ReadGeometry(Dictionary<string,Variant> geometry)
    {
        Variant geoTypeVar;
        Variant coordsVar;

        if (geometry.TryGetValue("type", out geoTypeVar))
        {
            if (geometry.TryGetValue("coordinates", out coordsVar))
            {
                return (geoTypeVar.AsString(), coordsVar.AsGodotArray<Variant>());

                
            }
        }

        return ("",[]);
    }


    // switch (geoType)
    // {
    //     case "Polygon":
    //         return (geoType,)
    //         break;
    //     case "MultiPolygon":

    //         break;
    //     case "LineString":

    //         break;
    //     case "MultiLineString":

    //         break;
    // }

    public static Array<Vector2> GetPolygonCoords(Array<Variant> coordsData)
    {
        Array<Vector2> coords = [];

        foreach (Array<double> coordData in coordsData[0].AsGodotArray<Array<double>>())
        {
            coords.Add(new Vector2((float)coordData[0],(float)coordData[1]));
        }

        return coords;
    }
}

