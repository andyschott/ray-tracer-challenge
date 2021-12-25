namespace RayTracerChallenge.Domain;

public class Sphere
{
    public Sphere()
    {
    }

    public IEnumerable<Intersection> Intersects(Ray ray)
    {
        // This assumes the sphere is centered at the world's origin
        var sphereToRay = ray.Origin - Tuple.CreatePoint(0, 0, 0);

        // Don't like these variable names, but not sure what to call them :(
        var a = (double)ray.Direction.DotProduct(ray.Direction);
        var b = (double)(2 * ray.Direction.DotProduct(sphereToRay));
        var c = (double)sphereToRay.DotProduct(sphereToRay) - 1;

        var discriminant = Math.Pow(b, 2) - 4.0 * a * c;
        if(discriminant < 0)
        {
            return Enumerable.Empty<Intersection>();
        }

        var t1 = (-b - Math.Sqrt(discriminant)) / (2 * a);
        var t2 = (-b + Math.Sqrt(discriminant)) / (2 * a);

        return new[] { t1, t2 }
            .OrderBy(t => t)
            .Select(t => new Intersection(Convert.ToDecimal(t), this))
            .ToArray();
    }
}