namespace RayTracerChallenge.Domain.Shapes;

public record Plane : Shape
{
    private static Tuple _normal = Tuple.CreateVector(0, 1, 0);
    public Plane(Matrix? transform = null,
        Material? material = null)
    : base(transform, material)
    {
    }
    
    protected override Intersections CalculateIntersection(Ray ray)
    {
        if (Math.Abs(ray.Direction.Y) < Constants.Epsilon)
        {
            return [];
        }
        
        var t = -ray.Origin.Y / ray.Direction.Y;
        return [new Intersection(t, this)];
    }

    protected override Tuple CalculateNormal(Tuple objectPoint)
    {
        return _normal;
    }
}