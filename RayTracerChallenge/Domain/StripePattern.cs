namespace RayTracerChallenge.Domain;

public record StripePattern : Pattern
{
    public Color A { get; init; }
    public Color B { get; init; }

    public StripePattern(Color a, Color b, Matrix? transform = null)
    : base(transform)
    {
        A = a;
        B = b;
    }
    
    public override Color ColorAt(Tuple point)
    {
        if (Math.Floor(point.X) % 2 is 0)
        {
            return A;
        }

        return B;
    }
}