using System.Security.AccessControl;
using RayTracerChallenge.Extensions;

namespace RayTracerChallenge.Domain;

public sealed record Color(
    decimal Red,
    decimal Green,
    decimal Blue)
{
    public static Color operator +(Color left, Color right)
    {
        return new Color(left.Red + right.Red,
            left.Green + right.Green,
            left.Blue + right.Blue);
    }

    public static Color operator -(Color left, Color right)
    {
        return new Color(left.Red - right.Red,
            left.Green - right.Green,
            left.Blue - right.Blue);
    }

    public static Color operator *(Color left, decimal right)
    {
        return new Color(left.Red * right,
            left.Green * right,
            left.Blue * right);
    }

    public static Color operator *(Color left, Color right)
    {
        return new Color(left.Red * right.Red,
            left.Green * right.Green,
            left.Blue * right.Blue);
    }

    public bool Equals(Color? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }
        
        return Red.IsEquivalent(other.Red) &&
               Green.IsEquivalent(other.Green) &&
               Blue.IsEquivalent(other.Blue);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Red, Green, Blue);
    }
}
    