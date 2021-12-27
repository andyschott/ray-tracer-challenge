namespace RayTracerChallenge.Domain;

public class Ray
{
    public Tuple Origin { get; }
    public Tuple Direction { get; }

    public Ray(Tuple origin, Tuple direction)
    {
        if(!origin.IsPoint)
        {
            throw new ArgumentException($"{nameof(origin)} must be a point", nameof(origin));
        }
        if(!direction.IsVector)
        {
            throw new ArgumentException($"{nameof(direction)} must be a vector", nameof(direction));
        }
        
        Origin = origin;
        Direction = direction;
    }

    public Tuple Position(decimal t)
    {
        return Origin + Direction * t;
    }

    public Ray Transform(Matrix transform)
    {
        return new Ray(transform * Origin, transform * Direction);
    }
}
