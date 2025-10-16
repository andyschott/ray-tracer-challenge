namespace RayTracerChallenge.Domain;

public record Cube : Shape
{
    public Cube(Matrix? transform = null,
        Material? material = null)
    : base(transform, material)
    {
    }
    
    protected override Intersections CalculateIntersection(Ray ray)
    {
        var (xMin, xMax) = CheckAxis(ray.Origin.X, ray.Direction.X);
        var (yMin, yMax) = CheckAxis(ray.Origin.Y, ray.Direction.Y);
        var (zMin, zMax) = CheckAxis(ray.Origin.Z, ray.Direction.Z);
        
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

    private static (double min, double max) CheckAxis(double origin, double direction)
    {
        var minNumerator = -1 - origin;
        var maxNumerator = 1 - origin;

        double min;
        double max;

        if (Math.Abs(direction) >= Constants.Epsilon)
        {
            min = minNumerator / direction;
            max = maxNumerator / direction;
        }
        else
        {
            min = minNumerator * double.PositiveInfinity;
            max = maxNumerator * double.PositiveInfinity;
        }

        if (min > max)
        {
            (min, max) = (max, min);
        }
        
        return (min, max);
    }
}