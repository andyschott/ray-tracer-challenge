namespace RayTracerChallenge.Domain;

public class StripePattern
{
    public Color First { get; }
    public Color Second { get; }

    public StripePattern(Color first, Color second)
    {
        First = first;
        Second = second;
    }

    public Color ColorAt(Tuple point)
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

    public Color this[Tuple point] => ColorAt(point);
}
