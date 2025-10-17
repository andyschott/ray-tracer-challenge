using RayTracerChallenge.Domain.Shapes;

namespace RayTracerChallenge.Domain.Patterns;

public abstract record Pattern
{
    protected Matrix Transform { get; }
    
    protected readonly Matrix InverseTransform;

    protected Pattern(Matrix? transform = null)
    {
        Transform = transform ?? Matrix.Identity;
        InverseTransform = Transform.Inverse();
    }
    
    public abstract Color ColorAt(Tuple point);

    public Color ColorAtForObject(Shape shape,
        Tuple point)
    {
        var objectPoint = shape.ConvertToObjectSpace(point);
        var patternPoint = InverseTransform * objectPoint;
        
        return ColorAt(patternPoint);
    }
}