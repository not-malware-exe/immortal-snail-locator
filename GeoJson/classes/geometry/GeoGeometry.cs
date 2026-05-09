using System.Drawing;
using System.Linq;
using Godot;
using Godot.Collections;

public partial class GeoGeometry : Node
{
    // public void importGeometryData(Dictionary<string,Variant> geometryData){}
    public virtual void ImportCoordsData(Array<Variant> coordsData){}

    public virtual Array<Vector2> GetGeometryPoints()
    {
        return [];
    }


    public virtual Array<Array<Vector2>> GetGeometryPointsArr()
    {
        return [[]];
    }

    public virtual Array<Vector2> GetGeometryTriangleStripPoints(float width)
    {
        return [];
    }

    public static Array<Vector2> GetGeometryTriangleStripPointsFromPoints(Array<Vector2> points, float width)
    {
        Array<Vector2> stripPoints = [];

        for (int i = 0; i < points.Count; i++)
        {
            Vector2 normal = GetNormalOfPoint(points,i);
            Vector2 point = points[i];

            stripPoints.Add(point + normal * width/2.0f);
            stripPoints.Add(point - normal * width/2.0f);
        }

        return stripPoints;
    }

    public static Vector2 GetNormalOfPoint(Array<Vector2> points, int i)
    {
        Vector2 p1;
        Vector2 p2;

        if (i > 0 && i < points.Count - 1)
        {
            p1 = points[i-1];
            p2 = points[i+1];
        }
        else if (i == 0)
        {
            p1 = points[i];
            p2 = points[i+1];
        }
        else if (i == points.Count - 1)
        {
            p1 = points[i-1];
            p2 = points[i];
        }
        else
            return Vector2.Zero;
        
        Vector2 dir = (p2 - p1).Normalized();

        return new Vector2(-dir.Y, dir.X);
    }

    public virtual Array GetGeometryPolygonIndices(int i_offset = 0)
    {
        return [];
    }

    public static Array GetGeometryPolygonIndicesFromPoints(Array<Vector2> points, int i_offset = 0)
    {
        Array<int> indices = [];

        for (int i = 0; i < points.Count; i++)
        {
            indices.Add(i+i_offset);
        }

        return [indices.ToArray()];
    }

    public virtual Array<int> GetGeometryTriangularIndices(int i_offset = 0)
    {
        return [];
    }

    public static Array<int> GetGeometryTriangularIndicesFromPoints(Array<Vector2> points, int i_offset = 0)
    {
        Array<int> indices = [];
        indices.AddRange(Geometry2D.TriangulatePolygon(points.ToArray()));

        for (int i = 0; i < indices.Count; i++)
        {
            indices[i] += i_offset;
        }

        return indices;
    }

    public virtual Array<int> GetGeometryLineIndices(int i_offset = 0)
    {
        return [];
    }

    public static Array<int> GetGeometryLineIndicesFromPoints(Array<Vector2> points, int i_offset = 0)
    {
        Array<int> indices = [];

        for (int i = 0; i < points.Count - 1; i++)
        {
            indices.Add(i+i_offset);
            indices.Add(i+i_offset+1);
        }

        return indices;
    }
    
    public virtual Array<int> GetGeometryTriangleStripIndices(int i_offset = 0)
    {
        return [];
    }

    public static Array<int> GetGeometryTriangleStripIndicesFromPoints(Array<Vector2> points, int i_offset = 0)
    {
        Array<int> indices = [];

        for (int i = 0; i < points.Count - 1; i++)
        {
            indices.Add((i+i_offset)*2);
            indices.Add((i+i_offset)*2+1);
            indices.Add((i+i_offset)*2+2);

            indices.Add((i+i_offset)*2+1);
            indices.Add((i+i_offset)*2+2);
            indices.Add((i+i_offset)*2+3);
        }

        return indices;
    }
}