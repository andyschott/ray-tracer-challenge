namespace RayTracerChallenge.Domain;

public class Material
{
    public Color Color { get; init; } = new Color
    {
        Red = 1,
        Green = 1,
        Blue = 1
    };

    public decimal Ambient { get; init; } = 0.1M;
    public decimal Diffuse { get; init; } = 0.9M;
    public decimal Specular { get; init; } = 0.9M;
    public decimal Shininess { get; init; } = 200.0M;
}
