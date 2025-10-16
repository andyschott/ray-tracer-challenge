namespace RayTracerChallenge.Domain;

public record RingPattern : Pattern
{
    public Color A { get; }
    public Color B { get; }

    public RingPattern(Color a, Color b, Matrix? transform = null)
    : base(transform)
    {
        A = a;
        B = b;
    }
    public override Color ColorAt(Tuple point)
    {
        var value = Math.Floor(Math.Sqrt(point.X * point.X + point.Z * point.Z));
        if (value % 2 is 0)
        {
            return A;
        }

        return B;
    }
}