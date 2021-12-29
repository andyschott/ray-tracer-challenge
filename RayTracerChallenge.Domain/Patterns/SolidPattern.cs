namespace RayTracerChallenge.Domain.Patterns;

public class SolidPattern : Pattern
{
    public Color Color { get; }

    public SolidPattern(Color color, Matrix? transform = null)
        : base(transform)
    {
        Color = color;
    }

    public override Color ColorAt(Tuple point) => Color;
}
