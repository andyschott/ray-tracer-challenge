namespace RayTracerChallenge.Domain;

public class Sphere
{
    public Matrix Transform { get; set; } = Matrix.Identity;
    
    public Intersections Intersects(Ray ray)
    {
        ray = ray.Transform(Transform.Inverse());
        
        // Assumes sphere is at the origin
        var sphereToRay = ray.Origin - Tuple.CreatePoint(0, 0, 0);
        
        var a = ray.Direction.Dot(ray.Direction);
        var b = 2 * ray.Direction.Dot(sphereToRay);
        var c = sphereToRay.Dot(sphereToRay) - 1;
        
        var discriminant = b * b - 4 * a * c;
        if (discriminant < 0)
        {
            return [];
        }

        var discriminantSqrt = (decimal)Math.Sqrt((double)discriminant);
        var doubleA = 2 * a;
        var t1 = (-b - discriminantSqrt) / doubleA;
        var t2 = (-b + discriminantSqrt) / doubleA;

        return
        [
            new Intersection(t1, this),
            new Intersection(t2, this),
        ];
    }}