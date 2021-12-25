namespace RayTracerChallenge.Domain;

public class Intersection
{
    public decimal T { get; init; }
    public Sphere Object { get; init ;}

    public Intersection(decimal t, Sphere s)
    {
        T = t;
        Object = s;
    }
}
