namespace RayTracerChallenge.Domain;
public record class Color
{
    public decimal Red { get; init; }
    public decimal Blue { get; init; }
    public decimal Green { get; init; }

    public Color Add(Color other)
    {
        return new Color
        {
            Red = Red + other.Red,
            Green = Green + other.Green,
            Blue = Blue + other.Blue
        };
    }

    public static Color operator +(Color color1, Color color2) => color1.Add(color2);

    public Color Subtract(Color other)
    {
        return new Color
        {
            Red = Red - other.Red,
            Green = Green - other.Green,
            Blue = Blue - other.Blue
        };
    }

    public static Color operator -(Color color1, Color color2) => color1.Subtract(color2);

    public Color Multiply(decimal scalar)
    {
        return new Color
        {
            Red = Red * scalar,
            Green = Green * scalar,
            Blue = Blue * scalar
        };
    }

    public static Color operator *(Color color, decimal scalar) => color.Multiply(scalar);

    public Color Multiply(Color other)
    {
        return new Color
        {
            Red = Red * other.Red,
            Green = Green * other.Green,
            Blue = Blue * other.Blue
        };
    }

    public static Color operator *(Color color1, Color color2) => color1.Multiply(color2);

    public override string ToString() => $"{Red}, {Green}, {Blue}";
}