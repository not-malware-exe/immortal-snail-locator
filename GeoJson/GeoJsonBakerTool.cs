
using System.Linq;
using Godot;
using Godot.Collections;

[GlobalClass]
[Tool]
public partial class GeoJsonBakerTool : Node
{
    [Export]
    public bool generateAndBakeGeometry
    {
        get => false;
        set
        {
            BakeGeometry();
        }
    }

    [Export]
    private MultiMeshInstance2D landMesh = null;
    [Export]
    private MultiMeshInstance2D riversMesh = null;
    [Export]
    private MultiMeshInstance2D lakesMesh = null;
    [Export]
    private MultiMeshInstance2D statesMesh = null;
    [Export]
    private MultiMeshInstance2D countriesMesh = null;

    public void BakeGeometry()
    {
        GeoJsonLoader geoJsonLoader = new GeoJsonLoader();

        float t0 = Time.GetTicksMsec();

        Dictionary<string,Variant> landJson = GeoJsonLoader.loadJson("res://GeoJson/ne_50m_land.json");
        Dictionary<string,Variant> lakesJson = GeoJsonLoader.loadJson("res://GeoJson/ne_10m_lakes.json");
        Dictionary<string,Variant> riversJson = GeoJsonLoader.loadJson("res://GeoJson/ne_10m_rivers_lake_centerlines.json");
        Dictionary<string,Variant> countriesJson = GeoJsonLoader.loadJson("res://GeoJson/ne_50m_admin_0_countries.json");
        Dictionary<string,Variant> statesJson = GeoJsonLoader.loadJson("res://GeoJson/ne_50m_admin_1_states_provinces.json");
        
        float t1 = Time.GetTicksMsec();
        GD.Print("Loading jsons (ms): ",t1-t0);

        geoJsonLoader.LoadGeoFeatures("land",landJson);
        geoJsonLoader.LoadGeoFeatures("lakes",lakesJson);
        geoJsonLoader.LoadGeoFeatures("rivers",riversJson);
        geoJsonLoader.LoadGeoFeatures("countries",countriesJson);
        geoJsonLoader.LoadGeoFeatures("states",statesJson);
        
        float t2 = Time.GetTicksMsec();
        GD.Print("Loading features (ms): ",t2-t1);

        (Array<Vector2> landPoints,Array<int> landIndices) = geoJsonLoader.GetFeatureCollectionTriPolyMeshData("land");
        (Array<Vector2> lakesPoints,Array<int> lakesIndices) = geoJsonLoader.GetFeatureCollectionTriPolyMeshData("lakes");
        (Array<Vector2> riversPoints,Array<int> riversIndices) = geoJsonLoader.GetFeatureCollectionTriStripMeshData("rivers");
        (Array<Vector2> countriesPoints,Array<int> countriesIndices) = geoJsonLoader.GetFeatureCollectionLineMeshData("countries");
        (Array<Vector2> statesPoints,Array<int> statesIndices) = geoJsonLoader.GetFeatureCollectionLineMeshData("states");
        
        float t3 = Time.GetTicksMsec();
        GD.Print("Getting mesh data (ms): ",t3-t2);

        GeoJsonLoader.SetMultiMeshInstance2DTriangleMesh(landMesh,landPoints,landIndices);
        GeoJsonLoader.SetMultiMeshInstance2DTriangleMesh(lakesMesh,lakesPoints,lakesIndices);
        GeoJsonLoader.SetMultiMeshInstance2DTriangleMesh(riversMesh,riversPoints,riversIndices);
        GeoJsonLoader.SetMultiMeshInstance2DLineMesh(countriesMesh,countriesPoints,countriesIndices);
        GeoJsonLoader.SetMultiMeshInstance2DLineMesh(statesMesh,statesPoints,statesIndices);
        
        float t4 = Time.GetTicksMsec();
        GD.Print("Setting meshes (ms): ",t4-t3);
        GD.Print("Total Time (ms): ",t4-t0);
    }
}

