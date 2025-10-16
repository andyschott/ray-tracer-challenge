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
}