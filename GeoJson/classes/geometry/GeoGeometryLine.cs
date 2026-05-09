using System.Linq;
using Godot;
using Godot.Collections;

public partial class GeoGeometryLine : GeoGeometry
{
    private Array<Vector2> points = [];

    public override void ImportCoordsData(Array<Variant> coordsData){
        Array<Vector2> coords = [];

        foreach (Variant coordDataVar in coordsData)
        {
            Array<double> coordData = coordDataVar.AsGodotArray<double>();
            coords.Add(new Vector2((float)coordData[0],(float)coordData[1]));
        }

        points = coords;
    }

    public override Array<Vector2> GetGeometryPoints()
    {
        return points;
    }

    public override Array<Array<Vector2>> GetGeometryPointsArr()
    {
        return [points];
    }
    
    public override Array<Vector2> GetGeometryTriangleStripPoints(float width)
    {
        return GetGeometryTriangleStripPointsFromPoints(points, width);
    }





    public override Array GetGeometryPolygonIndices(int i_offset = 0)
    {
        return GetGeometryPolygonIndicesFromPoints(points,i_offset);
    }

    public override Array<int> GetGeometryTriangularIndices(int i_offset = 0)
    {
        return GetGeometryTriangularIndicesFromPoints(points,i_offset);
    }

    public override Array<int> GetGeometryLineIndices(int i_offset = 0)
    {
        return GetGeometryLineIndicesFromPoints(points,i_offset);
    }

    public override Array<int> GetGeometryTriangleStripIndices(int i_offset = 0)
    {
        return GetGeometryTriangleStripIndicesFromPoints(points,i_offset);
    }
}