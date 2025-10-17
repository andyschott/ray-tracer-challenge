namespace RayTracerChallenge.Domain.Shapes;

public record Sphere : Shape
{
    private static readonly Bounds _bounds = new Bounds(-1, -1, -1,
        1, 1, 1);
    
    public Sphere(Matrix? transform = null,
        Material? material = null)
    : base(transform, material)
    {
    }

    public static Sphere Glass(Matrix?  transform = null,
        double transparency = 1,
        double refractiveIndex = 1.5)
    {
        return new Sphere(transform ?? Matrix.Identity)
        {
            Material = new Material
            {
                Transparency = transparency,
                RefractiveIndex = refractiveIndex,
            }
        };
    }
    
    protected override Intersections CalculateIntersection(Ray ray)
    {
        var sphereToRay = ray.Origin - Origin;
        
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

    protected override Tuple CalculateNormal(Tuple objectPoint)
    {
        return objectPoint - Origin;
    }

    public override Bounds GetBounds() => _bounds;
}