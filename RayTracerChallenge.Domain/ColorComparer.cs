using System.Diagnostics.CodeAnalysis;

namespace RayTracerChallenge.Domain;

public class ColorComparer : IEqualityComparer<Color>
{
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

        return x.Red == y.Red &&
            x.Green == y.Green &&
            x.Blue == y.Blue;
    }

    public int GetHashCode([DisallowNull] Color obj)
    {
        return HashCode.Combine(obj.Red, obj.Green, obj.Blue);
    }
}