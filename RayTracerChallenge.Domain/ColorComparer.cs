using System.Diagnostics.CodeAnalysis;

namespace RayTracerChallenge.Domain;

public class ColorComparer : IEqualityComparer<Color>
{
    private const decimal Epsilon = 0.0001M;

    public bool Equals(Color? x, Color? y)
    {
        if(x is null && y is null)
        {
            return true;
        }
        if(x is null || y is null)
        {
            return false;
        }

        return Math.Abs(x.Red - y.Red) <= Epsilon &&
            Math.Abs(x.Green - y.Green) <= Epsilon &&
            Math.Abs(x.Blue - y.Blue) <= Epsilon;
    }

    public int GetHashCode([DisallowNull] Color obj)
    {
        return HashCode.Combine(obj.Red, obj.Green, obj.Blue);
    }
}