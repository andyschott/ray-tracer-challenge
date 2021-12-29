namespace RayTracerChallenge.Domain;

public class GradientPattern : Pattern
{
    public Color Start { get; }
    public Color End { get; }

    private readonly Color _distance;

    public GradientPattern(Color start, Color end, Matrix? transform = null)
        : base(transform)
    {
        Start = start;
        End = end;

        _distance = End - Start;
    }

    public override Color ColorAt(Tuple point)
    {
        var fraction = point.X - Math.Floor(point.X);
        return Start + _distance * fraction;
    }
}
