namespace RayTracerChallenge.Domain;

public class Intersection
{
    public decimal T { get; init; }
    public Shape Object { get; init ;}

    public Intersection(decimal t, Shape s)
    {
        T = t;
        Object = s;
    }

    public IntersectionComputations PrepareComputations(Ray ray)
    {
        var point = ray.Position(T);
        var eyeVector = ray.Direction * -1;
        var normalVector = Object.NormalAt(point);
        var reflectVector = ray.Direction.Reflect(normalVector);

        return new IntersectionComputations(T, Object, point, eyeVector, normalVector, reflectVector);
    }
}
