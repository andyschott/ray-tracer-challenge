namespace RayTracerChallenge.Domain;

public record Ray(Tuple Origin, Tuple Direction)
{
    public Ray(double x, double y, double z,
        double vx, double vy, double vz)
    : this(Tuple.CreatePoint(x, y, z),
        Tuple.CreateVector(vx, vy, vz))
    {
    }
    public Tuple CalculatePosition(double t)
    {
        return Origin + Direction * t;
    }
    
    public Ray Transform(Matrix transformation)
    {
        var origin = transformation * Origin;
        var direction = transformation * Direction;
        
        return new Ray(origin, direction);
    }

    public bool Intersects(Bounds bounds)
    {
        var (xMin, xMax) = Helpers.CheckAxis(Origin.X, Direction.X,
            bounds.Minimum.X, bounds.Maximum.X);
        var (yMin, yMax) = Helpers.CheckAxis(Origin.Y, Direction.Y,
            bounds.Minimum.Y, bounds.Maximum.Y);
        var (zMin, zMax) = Helpers.CheckAxis(Origin.Z, Direction.Z,
            bounds.Minimum.Z, bounds.Maximum.Z);
        
        var min = Math.Max(xMin, Math.Max(yMin, zMin));
        var max = Math.Min(xMax, Math.Min(yMax, zMax));

        if (min > max)
        {
            return false;
        }

        return true;
    }
}