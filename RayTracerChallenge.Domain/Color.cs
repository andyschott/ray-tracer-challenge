namespace RayTracerChallenge.Domain;
public record class Color
{
    public float Red { get; init; }
    public float Blue { get; init; }
    public float Green { get; init; }

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

    public Color Multiply(float scalar)
    {
        return new Color
        {
            Red = Red * scalar,
            Green = Green * scalar,
            Blue = Blue * scalar
        };
    }

    public static Color operator *(Color color, float scalar) => color.Multiply(scalar);

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
}