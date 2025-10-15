namespace RayTracerChallenge.Domain;

public record Intersection(
    double T,
    Shape Shape)
{
    public record Computation(
        double T,
        Shape Shape,
        Tuple Point,
        Tuple EyeVector,
        Tuple NormalVector,
        bool IsInside,
        Tuple OverPoint);

    public Computation PrepareComputations(Ray ray)
    {
        var point = ray.CalculatePosition(T);
        var eyeVector = -ray.Direction;
        var normalVector = Shape.NormalAt(point);

        var inside = false;
        if (normalVector.Dot(eyeVector) < 0)
        {
            inside = true;
            normalVector = -normalVector;
        }
        
        var overPoint = point + normalVector * Constants.Epsilon;
        
        return new Computation(T,
            Shape,
            point,
            eyeVector,
            normalVector,
            inside,
            overPoint);
    }
}