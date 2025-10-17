namespace RayTracerChallenge.Domain.Shapes;

public record Cylinder : Shape
{
    public double Minimum { get; init; } = double.NegativeInfinity;
    public double Maximum { get; init; } = double.PositiveInfinity;
    public bool Closed { get; init; }
    
    public Cylinder(Matrix? transform = null,
        Material? material = null)
        : base(transform, material)
    {
    }

    protected override Intersections CalculateIntersection(Ray ray)
    {
        var a = Math.Pow(ray.Direction.X, 2) +
                Math.Pow(ray.Direction.Z, 2);
        
        var xs = new Intersections();

        if (a > Constants.Epsilon)
        {
            var b = 2 * ray.Origin.X * ray.Direction.X +
                    2 * ray.Origin.Z * ray.Direction.Z;
            var c = Math.Pow(ray.Origin.X, 2) +
                Math.Pow(ray.Origin.Z, 2) - 1;

            var discriminant = Math.Pow(b, 2) - 4 * a * c;

            // Ray does not intersect the cylinder
            if (discriminant < 0)
            {
                return [];
            }

            var t0 = (-1 * b - Math.Sqrt(discriminant)) / (2 * a);
            var t1 = (-1 * b + Math.Sqrt(discriminant)) / (2 * a);
            if (t0 > t1)
            {
                (t0, t1) = (t1, t0);
            }

            var y0 = ray.Origin.Y + t0 * ray.Direction.Y;
            if (y0 > Minimum && y0 < Maximum)
            {
                xs.Add(new Intersection(t0, this));
            }

            var y1 = ray.Origin.Y + t1 * ray.Direction.Y;
            if (y1 > Minimum && y1 < Maximum)
            {
                xs.Add(new Intersection(t1, this));
            }
        }

        IntersectCaps(ray, xs);

        return xs;
    }

    protected override Tuple CalculateNormal(Tuple objectPoint)
    {
        var dist = Math.Pow(objectPoint.X, 2) + Math.Pow(objectPoint.Z, 2);
        if (dist < 1)
        {
            if (objectPoint.Y >= Maximum - Constants.Epsilon)
            {
                return Tuple.CreateVector(0, 1, 0);
            }

            if (objectPoint.Y <= Minimum + Constants.Epsilon)
            {
                return Tuple.CreateVector(0, -1, 0);
            }
        }
        
        return Tuple.CreateVector(objectPoint.X, 0, objectPoint.Z);
    }

    private static bool ChecksCap(Ray ray, double t)
    {
        var x = ray.Origin.X + t * ray.Direction.X;
        var z = ray.Origin.Z + t * ray.Direction.Z;
        
        return (Math.Pow(x, 2) + Math.Pow(z, 2)) <= 1;
    }

    private void IntersectCaps(Ray ray, Intersections xs)
    {
        // Caps only matter if the cylinder is closed
        if (!Closed || Math.Abs(ray.Direction.Y) < Constants.Epsilon)
        {
            return;
        }
        
        // Check for intersection with the lower end cap by
        // intersecting the with the plane at Minimum
        var t0 = (Minimum - ray.Origin.Y) / ray.Direction.Y;
        if (ChecksCap(ray, t0))
        {
            xs.Add(new Intersection(t0, this));
        }
        
        // Check for intersection with upper end cap by
        // intersecting the ray with the play at Maximum
        var t1 = (Maximum - ray.Origin.Y) / ray.Direction.Y;
        if (ChecksCap(ray, t1))
        {
            xs.Add(new Intersection(t1, this));
        }
    }
}