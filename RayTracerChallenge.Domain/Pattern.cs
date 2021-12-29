namespace RayTracerChallenge.Domain;

public abstract class Pattern
{
    public abstract Color ColorAt(Tuple point);
    public Color this[Tuple point] => ColorAt(point);
}
