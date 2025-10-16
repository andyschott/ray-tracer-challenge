namespace RayTracerChallenge.Domain;

public record CheckersPattern : Pattern
{
    public Color A { get; }
    public Color B { get; }

    public CheckersPattern(Color a, Color b, Matrix? transform = null)
    : base(transform)
    {
        A = a;
        B = b;
    }
    
    public override Color ColorAt(Tuple point)
    {
        var value = Math.Floor(point.X) +
                    Math.Floor(point.Y) +
                    Math.Floor(point.Z);
        if (value % 2 is 0)
        {
            return A;
        }

        return B;
    }
}