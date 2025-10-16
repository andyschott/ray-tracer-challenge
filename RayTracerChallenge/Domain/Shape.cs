namespace RayTracerChallenge.Domain;

public abstract record Shape
{
    protected readonly Matrix InverseTransform;
    protected readonly Matrix InverseTransposeTransform;

    public Matrix Transform { get; }

    public Material Material { get; init; }
    
    protected readonly Tuple Origin = Tuple.CreatePoint(0, 0, 0);
    
    public Shape(Matrix? transform = null,
        Material? material = null)
    {
        Transform = transform ?? Matrix.Identity;
        InverseTransform = transform?.Inverse() ?? Matrix.Identity;
        InverseTransposeTransform = InverseTransform.Transpose();
        
        Material = material ?? new Material();
    }
    
    public Intersections Intersects(Ray ray)
    {
        ray = ray.Transform(InverseTransform);

        return CalculateIntersection(ray);
    }

    public Tuple ConvertToObjectSpace(Tuple point)
    {
        return InverseTransform * point;
    }
    
    public Tuple NormalAt(Tuple point)
    {
        var objectPoint = ConvertToObjectSpace(point);
        var objectNormal = CalculateNormal(objectPoint);

        // Should technically use Transform.Submatrix(3, 3) here
        // instead of Transform
        var worldNormal = InverseTransposeTransform * objectNormal;
        worldNormal = worldNormal with
        {
            W = 0
        };
        
        return worldNormal.Normalize();
    }

    protected abstract Intersections CalculateIntersection(Ray ray);
    protected abstract Tuple CalculateNormal(Tuple objectPoint);
}