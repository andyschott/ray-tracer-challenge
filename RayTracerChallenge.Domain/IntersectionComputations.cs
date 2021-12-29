namespace RayTracerChallenge.Domain;

public class IntersectionComputations
{
    private const decimal Epsilon = 0.0001M;

    public decimal T { get; init; }
    public Shape Object { get; init; }
    public Tuple Point { get; init; }
    public Tuple EyeVector { get; init; }
    public Tuple NormalVector { get; init; }
    public Tuple OverPoint { get; set; }
    public bool Inside { get; init; }

    public IntersectionComputations(decimal t, Shape obj,
        Tuple point, Tuple eyeVector, Tuple normalVector)
    {
        if(!point.IsPoint)
        {
            throw new ArgumentException($"{nameof(point)} must be a point");
        }
        if(!eyeVector.IsVector)
        {
            throw new ArgumentException($"{nameof(eyeVector)} must be a vector");
        }
        if(!normalVector.IsVector)
        {
            throw new ArgumentException($"{nameof(normalVector)} must be a vector");
        }

        T = t;
        Object = obj;
        Point = point;
        EyeVector = eyeVector;
        NormalVector = normalVector;

        Inside = normalVector.DotProduct(eyeVector) < 0;
        if(Inside)
        {
            NormalVector = NormalVector * -1;
        }

        OverPoint = Point + NormalVector * Epsilon;
    }
}
