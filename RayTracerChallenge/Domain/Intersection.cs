namespace RayTracerChallenge.Domain;

public record Intersection(
    double T,
    Sphere Sphere)
{
    public record Computation(
        double T,
        Sphere Object,
        Tuple Point,
        Tuple EyeVector,
        Tuple NormalVector,
        bool IsInside);

    public Computation PrepareComputations(Ray ray)
    {
        var point = ray.CalculatePosition(T);
        var eyeVector = -ray.Direction;
        var normalVector = Sphere.NormalAt(point);

        var inside = false;
        if (normalVector.Dot(eyeVector) < 0)
        {
            inside = true;
            normalVector = -normalVector;
        }
        
        return new Computation(T,
            Sphere,
            point,
            eyeVector,
            normalVector,
            inside);
    }
}