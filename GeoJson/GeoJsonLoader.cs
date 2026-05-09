
using System.Linq;
using Godot;
using Godot.Collections;

public partial class GeoJsonLoader : Node
{
    private Dictionary<string,Array<GeoFeature>> loadedGeoFeatures = [];

    public static Dictionary<string,Variant> loadJson(string path)
	{
		FileAccess file = FileAccess.Open(path, FileAccess.ModeFlags.Read);
		string jsonText = file.GetAsText();

		Dictionary<string,Variant> json = Json.ParseString(jsonText).AsGodotDictionary<string,Variant>();

        return json;
	}

    public void LoadGeoFeatures(string collectionName, Dictionary<string,Variant> geoJson)
    {
        if (!loadedGeoFeatures.ContainsKey(collectionName))
        {
            Array<Dictionary<string,Variant>> featuresData = GetFeaturesData(geoJson);
            Array<GeoFeature> geoFeatures = GetGeoFeatures(featuresData);
            loadedGeoFeatures.Add(collectionName,geoFeatures);
        }
    }

    public static Array<Dictionary<string,Variant>> GetFeaturesData(Dictionary<string,Variant> geoJson)
    {
        Variant featuresVar;
        
        if (geoJson.TryGetValue("features",out featuresVar))
        {
            return featuresVar.AsGodotArray<Dictionary<string,Variant>>();
        }

        return [];
    }

    public static Array<GeoFeature> GetGeoFeatures(Array<Dictionary<string,Variant>> featuresData)
    {
        Array<GeoFeature> geoFeatures = [];

        foreach (Dictionary<string,Variant> featureData in featuresData)
        {
            GeoFeature geoFeature = new GeoFeature();
            geoFeature.importFeatureData(featureData);
            geoFeatures.Add(geoFeature);
        }

        return geoFeatures;
    }

    public (Array<Vector2>,Array<int>) GetFeatureCollectionTriPolyMeshData(string collectionName)
    {
        Array<GeoFeature> features = loadedGeoFeatures[collectionName];
        Array<Vector2> points = [];
        Array<int> indices = [];

        foreach (GeoFeature feature in features)
        {
            GeoGeometry geoGeometry = feature.GetGeoGeometry();

            int i_offset = points.Count;
            points.AddRange(geoGeometry.GetGeometryPoints());
            indices.AddRange(geoGeometry.GetGeometryTriangularIndices(i_offset));
        }

        return (points, indices);
    }

    public (Array<Vector2>,Array<int>) GetFeatureCollectionTriStripMeshData(string collectionName, float width = 0.01f)
    {
        Array<GeoFeature> features = loadedGeoFeatures[collectionName];
        Array<Vector2> points = [];
        Array<int> indices = [];

        foreach (GeoFeature feature in features)
        {
            GeoGeometry geoGeometry = feature.GetGeoGeometry();

            int i_offset = points.Count;
            points.AddRange(geoGeometry.GetGeometryTriangleStripPoints(width));
            indices.AddRange(geoGeometry.GetGeometryTriangleStripIndices(i_offset/2));
        }

        return (points, indices);
    }
    
    public (Array<Vector2>,Array<int>) GetFeatureCollectionLineMeshData(string collectionName)
    {
        Array<GeoFeature> features = loadedGeoFeatures[collectionName];
        Array<Vector2> points = [];
        Array<int> indices = [];

        foreach (GeoFeature feature in features)
        {
            GeoGeometry geoGeometry = feature.GetGeoGeometry();

            int i_offset = points.Count;
            points.AddRange(geoGeometry.GetGeometryPoints());
            indices.AddRange(geoGeometry.GetGeometryLineIndices(i_offset));
        }

        return (points, indices);
    }

    public static void SetMultiMeshInstance2DTriangleMesh(MultiMeshInstance2D multiMeshInstance, Array<Vector2> points, Array<int> pointIndices)
    {
        // 1. Create the Mesh
        ArrayMesh mesh = new ArrayMesh();
        Array arrays = new Array();
        arrays.Resize((int)Mesh.ArrayType.Max);

        Vector2[] vertices = points.ToArray();
        int[] indices = pointIndices.ToArray();

        arrays[(int)Mesh.ArrayType.Vertex] = vertices;
        arrays[(int)Mesh.ArrayType.Index] = indices;
        mesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, arrays);
        
        // 2. Set up the MultiMesh
        multiMeshInstance.Multimesh = new MultiMesh();
        multiMeshInstance.Multimesh.Mesh = mesh;
        multiMeshInstance.Multimesh.InstanceCount = 1; // 
        multiMeshInstance.Multimesh.SetInstanceTransform2D(0, new Transform2D(0, Vector2.Zero));
    }

    public static void SetMultiMeshInstance2DLineMesh(MultiMeshInstance2D multiMeshInstance, Array<Vector2> points, Array<int> pointIndices)
    {
        // 1. Create the Mesh
        ArrayMesh mesh = new ArrayMesh();
        Array arrays = new Array();
        arrays.Resize((int)Mesh.ArrayType.Max);

        Vector2[] vertices = points.ToArray();
        int[] indices = pointIndices.ToArray();

        arrays[(int)Mesh.ArrayType.Vertex] = vertices;
        arrays[(int)Mesh.ArrayType.Index] = indices;
        mesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Lines, arrays);
        
        // 2. Set up the MultiMesh
        multiMeshInstance.Multimesh = new MultiMesh();
        multiMeshInstance.Multimesh.Mesh = mesh;
        multiMeshInstance.Multimesh.InstanceCount = 1; // 
        multiMeshInstance.Multimesh.SetInstanceTransform2D(0, new Transform2D(0, Vector2.Zero));
    }
}

