namespace RayTracerChallenge.Domain;

public record GradientPattern : Pattern
{
    public Color A { get; }
    public Color B { get; }

    private readonly Color _distance;
    
    public GradientPattern(Color a, Color b, Matrix? transform = null)
    : base(transform)
    {
        A = a;
        B = b;

        _distance = B - A;
    }
    
    public override Color ColorAt(Tuple point)
    {
        var fraction = point.X - Math.Floor(point.X);
        return A + _distance * fraction;
    }
}