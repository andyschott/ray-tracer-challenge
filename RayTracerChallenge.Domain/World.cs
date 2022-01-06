using RayTracerChallenge.Domain.Extensions;
using RayTracerChallenge.Domain.Patterns;

namespace RayTracerChallenge.Domain;

public class World
{
    public Light? Light { get; set; } = null;
    public IList<Shape> Objects { get; set; } = new List<Shape>();

    private static readonly TransformationFactory _factory = new TransformationFactory();

    public static World Default(Light? light = null, Material? material1 = null, Material? material2 = null)
    {
        light ??= new Light(Tuple.CreatePoint(-10, 10, -10), new Color
        {
            Red = 1,
            Green = 1,
            Blue = 1,
        });
        var s1 = new Sphere
        {
            Material = material1 ?? new Material
            {
                Diffuse = 0.7M,
                Specular = 0.2M,
                Pattern = new SolidPattern(new Color
                {
                    Red = 0.8M,
                    Green = 1.0M,
                    Blue = 0.6M
                })
            }
        };
        var s2 = new Sphere
        {
            Material = material2 ?? new Material(),
            Transform = _factory.Scale(0.5M, 0.5M, 0.5M)
        };

        return new World
        {
            Light = light,
            Objects = new List<Shape> { s1, s2 }
        };
    }

    public IEnumerable<Intersection> Intersect(Ray ray)
    {
        return Objects.SelectMany(obj => obj.Intersects(ray))
            .OrderBy(intersection => intersection.T);
    }

    public Color ShadeHit(IntersectionComputations computations)
    {
        if(Light is null)
        {
            throw new NullReferenceException($"{nameof(Light)} must not be null");
        }
        var isShadowed = IsShadowed(computations.OverPoint);

        var surface = computations.Object.Material.Lighting(computations.Object, Light,
            computations.Point, computations.EyeVector, computations.NormalVector,
            isShadowed);
        var reflected = ReflectedColor(computations);

        return surface + reflected;
    }

    public Color ColorAt(Ray ray)
    {
        var intersections = Intersect(ray);
        var hit = intersections.Hit();
        if(hit is null)
        {
            return Color.Black;
        }

        var computations = hit.PrepareComputations(ray);
        return ShadeHit(computations);
    }

    public Color ReflectedColor(IntersectionComputations computations)
    {
        if(computations.Object.Material.Reflective == 0.0M)
        {
            return Color.Black;
        }

        var reflectedRay = new Ray(computations.OverPoint, computations.ReflectVector);
        var color = ColorAt(reflectedRay);

        return color * computations.Object.Material.Reflective;
    }

    public bool IsShadowed(Tuple point)
    {
        if(!point.IsPoint)
        {
            throw new ArgumentException($"{nameof(point)} must be a point");
        }

        if(Light is null)
        {
            return false;
        }

        var vector = Light.Position - point;
        var distance = vector.Magnitude();
        var direction = vector.Normalize();

        var ray = new Ray(point, direction);
        var intersections = Intersect(ray);

        var hit = intersections.Hit();
        return hit?.T < distance;
    }
}
