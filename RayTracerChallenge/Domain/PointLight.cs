namespace RayTracerChallenge.Domain;

public record PointLight(Tuple Position, Color Intensity)
{
    public PointLight(double x, double y, double z, double r, double g, double b)
    : this(Tuple.CreatePoint(x, y, z), new Color(r, g, b))
    {
    }
}