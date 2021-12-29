namespace RayTracerChallenge.Domain.Patterns;

public class StripePattern : Pattern
{
    public Color First { get; }
    public Color Second { get; }

    public StripePattern(Color first, Color second, Matrix? transform = null)
        : base(transform)
    {
        First = first;
        Second = second;
    }

    public override Color ColorAt(Tuple point)
    {
        if(!point.IsPoint)
        {
            throw new ArgumentException($"{nameof(point)} must be a point");
        }

        if(Math.Floor(point.X) % 2 == 0)
        {
            return First;
        }

        return Second;
    }
}
