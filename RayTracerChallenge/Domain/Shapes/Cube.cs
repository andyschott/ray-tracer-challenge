namespace RayTracerChallenge.Domain.Shapes;

public record Cube : Shape
{
    private static readonly Bounds _bounds = new Bounds(-1, -1, -1,
        1, 1, 1);

    public Cube(Matrix? transform = null,
        Material? material = null)
    : base(transform, material)
    {
    }
    
    protected override Intersections CalculateIntersection(Ray ray)
    {
        var (xMin, xMax) = Helpers.CheckAxis(ray.Origin.X, ray.Direction.X,
            _bounds.Minimum.X, _bounds.Maximum.X);
        var (yMin, yMax) = Helpers.CheckAxis(ray.Origin.Y, ray.Direction.Y,
            _bounds.Minimum.Y, _bounds.Maximum.Y);
        var (zMin, zMax) = Helpers.CheckAxis(ray.Origin.Z, ray.Direction.Z,
            _bounds.Minimum.Z, _bounds.Maximum.Z);
        
        var min = Math.Max(xMin, Math.Max(yMin, zMin));
        var max = Math.Min(xMax, Math.Min(yMax, zMax));

        if (min > max)
        {
            return [];
        }

        return
        [
            new Intersection(min, this),
            new Intersection(max, this)
        ];
    }

    protected override Tuple CalculateNormal(Tuple objectPoint)
    {
        var absX = Math.Abs(objectPoint.X);
        var absY = Math.Abs(objectPoint.Y);
        var absZ = Math.Abs(objectPoint.Z);
        
        var maxc = Math.Max(absX, Math.Max(absY, absZ));
        if (maxc - absX < Constants.Epsilon)
        {
            return Tuple.CreateVector(objectPoint.X, 0, 0);
        }

        if (maxc - absY < Constants.Epsilon)
        {
            return Tuple.CreateVector(0, objectPoint.Y, 0);
        }
        
        return Tuple.CreateVector(0, 0, objectPoint.Z);
    }

    public override Bounds GetBounds() => _bounds;
}