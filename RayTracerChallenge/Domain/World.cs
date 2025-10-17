using RayTracerChallenge.Domain.Shapes;
using RayTracerChallenge.Extensions;

namespace RayTracerChallenge.Domain;

public class World
{
    private readonly List<Shape> _shapes = [];

    public IList<Shape> Shapes => _shapes;
    public PointLight? Light { get; set; }

    public static PointLight DefaultLight { get; } =
        new(Tuple.CreatePoint(-10, 10, -10),
            new Color(1, 1, 1));
    public static World DefaultWorld(Material? shape1Material = null,
        Material? shape2Material = null)
    {
        var s1 = new Sphere
        {
            Material = shape1Material ?? new Material
            {
                Color = new Color(0.8, 1.0, 0.6),
                Diffuse = 0.7,
                Specular = 0.2,
            }
        };
        var s2 = new Sphere(Matrix.Identity
            .Scale(0.5, 0.5, 0.5))
        {
            Material = shape2Material ?? new Material()
        };

        var w = new World
        {
            Light = DefaultLight
        };
        w.Shapes.Add(s1);
        w.Shapes.Add(s2);

        return w;
    }

    public Intersections Intersect(Ray ray)
    {
        var intersections = new List<Intersection>();

        foreach (var obj in Shapes)
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

    public Color ShadeHit(Intersection.Computation comps, int remaining = 5)
    {
        if (Light is null)
        {
            return new Color(0, 0, 0);
        }

        var isShadowed = IsShadowed(comps.OverPoint);
        
        var surface= comps.Shape.Material.Lighting(comps.Shape,
            Light,
            comps.OverPoint,
            comps.EyeVector,
            comps.NormalVector,
            isShadowed);

        var reflected = ReflectedColor(comps, remaining);
        var refracted = RefractedColor(comps, remaining);
        
        if (comps.Shape.Material is { Reflective: > 0, Transparency: > 0 })
        {
            var reflectance = comps.Schlick();
            return surface + reflected * reflectance +
                   refracted * (1 - reflectance);
        }
        
        return surface + reflected + refracted;
    }

    public Color ColorAt(Ray ray, int remaining = 5)
    {
        var i = Intersect(ray);
        var hit = i.Hit();
        if (hit is null)
        {
            return new Color(0, 0, 0);
        }

        var comps = hit.PrepareComputations(ray, i);
        return ShadeHit(comps, remaining);
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

    public Color ReflectedColor(Intersection.Computation comps, int remaining = 5)
    {
        if (remaining <= 0 || comps.Shape.Material.Reflective is 0)
        {
            return new Color(0, 0, 0);
        }
        
        var reflectRay = new Ray(comps.OverPoint, comps.ReflectionVector);
        var color = ColorAt(reflectRay, remaining - 1);

        return color * comps.Shape.Material.Reflective;
    }

    public Color RefractedColor(Intersection.Computation comps, int remaining = 5)
    {
        if (remaining is 0 || comps.Shape.Material.Transparency is 0)
        {
            return new Color(0, 0, 0);
        }

        // Find the ratio of the first index of refraction to the second
        var nRatio = comps.NRatio;
        
        var cos_i = comps.EyeVector.Dot(comps.NormalVector);
        var sin2_t = nRatio * nRatio * (1 - cos_i * cos_i);
        
        // Check for total internal refraction
        if (sin2_t > cos_i)
        {
            return new Color(0, 0, 0);
        }
        
        // Compute the direction of the refracted ray
        var cos_t = Math.Sqrt(1 - sin2_t);
        var direction = comps.NormalVector * (nRatio * cos_i - cos_t) -
                        comps.EyeVector * nRatio;
        var refractedRay = new Ray(comps.UnderPoint, direction);
        
        // Find the color of the refracted ray, making sure to multiply
        // by the transparency value to account for any opacity
        var refractedColor = ColorAt(refractedRay, remaining - 1) *
                             comps.Shape.Material.Transparency;

        return refractedColor;
    }
}