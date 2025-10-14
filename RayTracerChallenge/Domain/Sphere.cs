namespace RayTracerChallenge.Domain;

public record Sphere
{
    private readonly Matrix _inverseTransform;
    private readonly Matrix _inverseTransposeTransform;

    public Matrix Transform { get; }

    public Material Material { get; init; }
    
    private readonly Tuple _origin = Tuple.CreatePoint(0, 0, 0);

    public Sphere(Matrix? transform = null,
        Material? material = null)
    {
        Transform = transform ?? Matrix.Identity;
        _inverseTransform = transform?.Inverse() ?? Matrix.Identity;
        _inverseTransposeTransform = _inverseTransform.Transpose();
        
        Material = material ?? new Material();
    }
    
    public Intersections Intersects(Ray ray)
    {
        ray = ray.Transform(_inverseTransform);
        
        var sphereToRay = ray.Origin - _origin;
        
        var a = ray.Direction.Dot(ray.Direction);
        var b = 2 * ray.Direction.Dot(sphereToRay);
        var c = sphereToRay.Dot(sphereToRay) - 1;
        
        var discriminant = b * b - 4 * a * c;
        if (discriminant < 0)
        {
            return [];
        }

        var discriminantSqrt = Math.Sqrt(discriminant);
        var doubleA = 2 * a;
        var t1 = (-b - discriminantSqrt) / doubleA;
        var t2 = (-b + discriminantSqrt) / doubleA;

        return
        [
            new Intersection(t1, this),
            new Intersection(t2, this),
        ];
    }

    public Tuple NormalAt(Tuple point)
    {
        var objectPoint = _inverseTransform * point;
        var objectNormal = objectPoint - _origin;

        // Should technically use Transform.Submatrix(3, 3) here
        // instead of Transform
        var worldNormal = _inverseTransposeTransform * objectNormal;
        worldNormal = worldNormal with
        {
            W = 0
        };
        
        return worldNormal.Normalize();
    }
}