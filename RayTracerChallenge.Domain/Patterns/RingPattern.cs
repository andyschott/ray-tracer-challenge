namespace RayTracerChallenge.Domain.Patterns;

public class RingPattern : Pattern
{
    public Color First { get; }
    public Color Second { get; }
    
    public RingPattern(Color first, Color second, Matrix? transform = null)
        : base(transform)
    {
        First = first;
        Second = second;
    }
    
    public override Color ColorAt(Tuple point)
    {
        var px = Math.Pow((double)point.X, 2);
        var pz = Math.Pow((double)point.Z, 2);

        if(Math.Floor(Math.Sqrt(px + pz)) % 2 == 0)
        {
            return First;
        }

        return Second;
    }
}
