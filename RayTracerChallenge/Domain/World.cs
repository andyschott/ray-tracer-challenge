using RayTracerChallenge.Extensions;

namespace RayTracerChallenge.Domain;

public class World
{
    private readonly List<Sphere> _objects = [];

    public IList<Sphere> Objects => _objects;
    public PointLight? Light { get; set; }

    public static PointLight DefaultLight { get; } =
        new PointLight(Tuple.CreatePoint(-10, 10, -10),
            new Color(1, 1, 1));
    public static World DefaultWorld()
    {
        var s1 = new Sphere
        {
            Material = new Material
            {
                Color = new Color(0.8, 1.0, 0.6),
                Diffuse = 0.7,
                Specular = 0.2,
            }
        };
        var s2 = new Sphere(Matrix.Identity
            .Scale(0.5, 0.5, 0.5));

        var w = new World
        {
            Light = DefaultLight
        };
        w._objects.Add(s1);
        w._objects.Add(s2);

        return w;
    }

    public Intersections Intersect(Ray ray)
    {
        var intersections = new List<Intersection>();

        foreach (var obj in Objects)
        {
            var xs = obj.Intersects(ray);
            var hits = xs.Hits();
            intersections.AddRange(hits);
        }
        intersections.Sort((left, right) =>
        {
            if (left.T < right.T)
            {
                return -1;
            }

            if (left.T > right.T)
            {
                return 1;
            }

            return 0;
        });

        return new Intersections(intersections);
    }

    public Color ShadeHit(Intersection.Computation comps)
    {
        if (Light is null)
        {
            return new Color(0, 0, 0);
        }

        var isShadowed = IsShadowed(comps.OverPoint);
        
        return comps.Object.Material.Lighting(Light,
            comps.Point,
            comps.EyeVector,
            comps.NormalVector,
            isShadowed);
    }

    public Color ColorAt(Ray ray)
    {
        var i = Intersect(ray);
        var hit = i.Hit();
        if (hit is null)
        {
            return new Color(0, 0, 0);
        }

        var comps = hit.PrepareComputations(ray);
        return ShadeHit(comps);
    }

    public bool IsShadowed(Tuple point)
    {
        if (Light is null)
        {
            return true;
        }
        
        var v = Light.Position - point;
        var distance = v.Magnitude;
        var direction = v.Normalize();
        
        var r = new Ray(point, direction);
        var xs = Intersect(r);
        var hit = xs.Hit();
        return hit?.T < distance;
    }
}