namespace RayTracerChallenge.Domain.Patterns;

public abstract class Pattern
{
    public Matrix Transform { get; }

    protected Pattern(Matrix? transform = null)
    {
        Transform = transform ?? Matrix.Identity();
    }

    public abstract Color ColorAt(Tuple point);
    public Color this[Tuple point] => ColorAt(point);

    public Color ColorAt(Shape shape, Tuple point)
    {
        var objectPoint = shape.Transform.Invert() * point;
        var patternPoint = Transform.Invert() * objectPoint;

        return ColorAt(patternPoint);
    }
}
