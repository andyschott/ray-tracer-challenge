namespace RayTracerChallenge.Domain;

public record Bounds(Tuple Minimum, Tuple Maximum)
{
    public Bounds(double minX, double minY, double minZ,
        double maxX, double maxY, double maxZ)
    : this(Tuple.CreatePoint(minX, minY, minZ),
        Tuple.CreatePoint(maxX, maxY, maxZ))
    {
    }
}