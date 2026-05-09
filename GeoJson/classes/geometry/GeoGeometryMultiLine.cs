using System.Linq;
using Godot;
using Godot.Collections;

public partial class GeoGeometryMultiLine : GeoGeometry
{
    private Array<Array<Vector2>> pointsArr = [];

    public override void ImportCoordsData(Array<Variant> coordsData){
        Array<Array<Vector2>> coordsArr = [];

        foreach (Variant coordDataArrVar in coordsData)
        {
            Array<Variant> coordDataArr = coordDataArrVar.AsGodotArray<Variant>();
            Array<Vector2> coords = [];
            int pointCount = 0;

            foreach (Variant coordDataVar in coordDataArr)
            {
                Array<double> coordData = coordDataVar.AsGodotArray<double>();
                coords.Add(new Vector2((float)coordData[0],(float)coordData[1]));
                pointCount++;
            }

            coordsArr.Add(coords);
        }

        
        pointsArr = coordsArr;
    }

    public override Array<Vector2> GetGeometryPoints()
    {
        Array<Vector2> accumulatedPoints = [];

        foreach (Array<Vector2> points in pointsArr)
        {
            accumulatedPoints.AddRange(points);
        }

        return accumulatedPoints;
    }

    public override Array<Array<Vector2>> GetGeometryPointsArr()
    {
        return pointsArr;
    }
    
    public override Array<Vector2> GetGeometryTriangleStripPoints(float width)
    {
        Array<Vector2> accumulatedPoints = [];

        foreach (Array<Vector2> points in pointsArr)
        {
            accumulatedPoints.AddRange(GetGeometryTriangleStripPointsFromPoints(points, width));
        }

        return accumulatedPoints;
    }





    public override Array GetGeometryPolygonIndices(int i_offset = 0)
    {
        Array indicesArr = [];
        int accumulatedPointCount = 0;

        foreach (Array<Vector2> points in pointsArr)
        {
            indicesArr.Add(GetGeometryPolygonIndicesFromPoints(points,i_offset + accumulatedPointCount));
            accumulatedPointCount += points.Count;
        }

        return indicesArr;
    }

    public override Array<int> GetGeometryTriangularIndices(int i_offset = 0)
    {
        Array<int> indices = [];
        int accumulatedPointCount = 0;

        foreach (Array<Vector2> points in pointsArr)
        {
            indices.AddRange(GetGeometryTriangularIndicesFromPoints(points,i_offset + accumulatedPointCount));
            accumulatedPointCount += points.Count;
        }

        return indices;
    }

    public override Array<int> GetGeometryLineIndices(int i_offset = 0)
    {
        Array<int> indices = [];
        int accumulatedPointCount = 0;

        foreach (Array<Vector2> points in pointsArr)
        {
            indices.AddRange(GetGeometryLineIndicesFromPoints(points,i_offset + accumulatedPointCount));
            accumulatedPointCount += points.Count;
        }

        return indices;
    }
    
    public override Array<int> GetGeometryTriangleStripIndices(int i_offset = 0)
    {
        Array<int> indices = [];
        int accumulatedPointCount = 0;

        foreach (Array<Vector2> points in pointsArr)
        {
            indices.AddRange(GetGeometryTriangleStripIndicesFromPoints(points,i_offset + accumulatedPointCount));
            accumulatedPointCount += points.Count;
        }

        return indices;
    }
}