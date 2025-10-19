namespace RayTracerChallenge.Domain.Shapes;

public abstract record Shape
{
    protected readonly Matrix InverseTransform;
    protected readonly Matrix InverseTransposeTransform;

    public Matrix Transform { get; }

    private static readonly Material _defaultMaterial = new();
    private readonly Material? _material;
    public Material Material
    {
        get
        {
            if (_material is null)
            {
                return Parent?.Material ?? _defaultMaterial;
            }
            return _material;
        }
        init => _material = value;
    }
    
    public Shape? Parent { get; set; }
    
    protected readonly Tuple Origin = Tuple.CreatePoint(0, 0, 0);
    
    protected Shape(Matrix? transform = null,
        Material? material = null)
    {
        Transform = transform ?? Matrix.Identity;
        InverseTransform = transform?.Inverse() ?? Matrix.Identity;
        InverseTransposeTransform = InverseTransform.Transpose();
        
        _material = material;
    }
    
    public Intersections Intersects(Ray ray)
    {
        ray = ray.Transform(InverseTransform);

        return CalculateIntersection(ray);
    }

    public Tuple ConvertToObjectSpace(Tuple point)
    {
        if (Parent is not null)
        {
            point = Parent.ConvertToObjectSpace(point);
        }
        return InverseTransform * point;
    }

    public Tuple ConvertNormalToWorld(Tuple normal)
    {
        var worldNormal = InverseTransposeTransform * normal;
        worldNormal = worldNormal with
        {
            W = 0
        };
        worldNormal = worldNormal.Normalize();

        if (Parent is not null)
        {
            worldNormal = Parent.ConvertNormalToWorld(worldNormal);
        }
        
        return worldNormal;
    }
    
    public Tuple NormalAt(Tuple point, Intersection hit)
    {
        var objectPoint = ConvertToObjectSpace(point);
        var objectNormal = CalculateNormal(objectPoint, hit);

        return ConvertNormalToWorld(objectNormal);
    }
    
    public abstract Bounds GetBounds();

    protected abstract Intersections CalculateIntersection(Ray ray);
    protected abstract Tuple CalculateNormal(Tuple objectPoint,
        Intersection hit);
}