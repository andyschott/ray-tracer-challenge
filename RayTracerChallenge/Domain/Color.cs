using System.Security.AccessControl;

namespace RayTracerChallenge.Domain;

public record Color(
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
}
    