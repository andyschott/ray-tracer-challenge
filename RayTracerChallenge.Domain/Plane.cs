namespace RayTracerChallenge.Domain;

public class Plane : Shape
{
    private const decimal Epsilon = 0.0001M;

    protected override IEnumerable<Intersection> LocalIntersects(Ray ray)
    {
        if(Math.Abs(ray.Direction.Y) < Epsilon)
        {
            return Enumerable.Empty<Intersection>();
        }

        var t = -ray.Origin.Y / ray.Direction.Y;
        return new[] { new Intersection(t, this) };
    }

    protected override Tuple LocalNormalAt(Tuple point)
    {
        return Tuple.CreateVector(0, 1, 0);
    }
}
