using System.Collections;

namespace RayTracerChallenge.Domain.Shapes;

public record Triangle : Shape
{
    public Tuple Point1 { get; }
    public Tuple Point2 { get; }
    public Tuple Point3 { get; }
    
    public Tuple Edge1 { get; }
    public Tuple Edge2 { get; }
    public Tuple Normal { get; }

    public Triangle(Tuple point1, Tuple point2, Tuple point3,
        Matrix? transform = null,
        Material? material = null)
    : base(transform, material)
    {
        Point1 = point1;
        Point2 = point2;
        Point3 = point3;

        Edge1 = Point2 - Point1;
        Edge2 = Point3 - Point1;
        Normal = Edge2.Cross(Edge1).Normalize();
    }

    public Triangle(double p1x, double p1y, double p1z,
        double p2x, double p2y, double p2z,
        double p3x, double p3y, double p3z,
        Matrix? transform = null,
        Material? material = null)
    : this(Tuple.CreatePoint(p1x, p1y, p1z),
        Tuple.CreatePoint(p2x, p2y, p2z),
        Tuple.CreatePoint(p3x, p3y, p3z),
        transform,
        material)
    {
    }
    
    public override Bounds GetBounds()
    {
        throw new NotImplementedException();
    }

    protected override Intersections CalculateIntersection(Ray ray)
    {
        var directionCrossEdge2 = ray.Direction.Cross(Edge2);
        var determinant = Edge1.Dot(directionCrossEdge2);

        if (Math.Abs(determinant) < double.Epsilon)
        {
            return [];
        }

        var f = 1.0 / determinant;
        var p1ToOrigin = ray.Origin - Point1;
        var u = f * p1ToOrigin.Dot(directionCrossEdge2);
        if (u is < 0 or > 1)
        {
            return [];
        }

        var originCrossEdge1 = p1ToOrigin.Cross(Edge1);
        var v = f * ray.Direction.Dot(originCrossEdge1);
        if (v < 0 || (u + v) > 1) 
        {
            return [];
        }

        var t = f * Edge2.Dot(originCrossEdge1);
        return
        [
            new Intersection(t, this, u, v)
        ];
    }

    protected override Tuple CalculateNormal(Tuple objectPoint, Intersection hit)
    {
        return Normal;
    }
}