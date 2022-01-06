namespace RayTracerChallenge.Domain.Patterns;

public class CheckerPattern : Pattern
{
    public Color First { get; }
    public Color Second { get; }
    
    public CheckerPattern(Color first, Color second, Matrix? transform = null)
        : base(transform)
    {
        First = first;
        Second = second;
    }

    public override Color ColorAt(Tuple point)
    {
        var sum = Math.Floor(point.X) + Math.Floor(point.Y) + Math.Floor(point.Z);
        if(sum % 2 == 0)
        {
            return First;
        }

        return Second;
    }
}
