namespace RayTracerChallenge.Domain.Shapes;

public record SmoothTriangle : Triangle
{
    public Tuple Normal1 { get; }
    public Tuple Normal2 { get; }
    public Tuple Normal3 { get; }

    public SmoothTriangle(Tuple point1, Tuple point2, Tuple point3,
        Tuple normal1, Tuple normal2, Tuple normal3,
        Matrix? transform = null, Material? material = null)
        : base(point1, point2, point3, transform, material)
    {
        Normal1 = normal1;
        Normal2 = normal2;
        Normal3 = normal3;
    }

    public SmoothTriangle(double p1x, double p1y, double p1z,
        double p2x, double p2y, double p2z,
        double p3x, double p3y, double p3z,
        double n1x, double n1y, double n1z,
        double n2x, double n2y, double n2z,
        double n3x, double n3y, double n3z,
        Matrix? transform = null, Material? material = null)
        : this(Tuple.CreatePoint(p1x, p1y, p1z),
            Tuple.CreatePoint(p2x, p2y, p2z),
            Tuple.CreatePoint(p3x, p3y, p3z),
            Tuple.CreateVector(n1x, n1y, n1z),
            Tuple.CreateVector(n2x, n2y, n2z),
            Tuple.CreateVector(n3x, n3y, n3z),
            transform,
            material)
    {
    }

    protected override Tuple CalculateNormal(Tuple objectPoint, Intersection hit)
    {
        ArgumentNullException.ThrowIfNull(hit.U);
        ArgumentNullException.ThrowIfNull(hit.V);

        return Normal2 * hit.U.Value +
               Normal3 * hit.V.Value +
               Normal1 * (1 - hit.U.Value - hit.V.Value);
    }
}