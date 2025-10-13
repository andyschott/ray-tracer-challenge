namespace RayTracerChallenge.Domain;

public record Ray(Tuple Origin, Tuple Direction)
{
    public Tuple CalculatePosition(decimal t)
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