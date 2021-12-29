using RayTracerChallenge.Domain.Extensions;

namespace RayTracerChallenge.Domain;

public class World
{
    public Light? Light { get; set; } = null;
    public IEnumerable<Shape> Objects { get; set; } = Enumerable.Empty<Shape>();

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
                Color = new Color
                {
                    Red = 0.8M,
                    Green = 1.0M,
                    Blue = 0.6M
                },
                Diffuse = 0.7M,
                Specular = 0.2M
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
            Objects = new[] { s1, s2 }
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

        return computations.Object.Material.Lighting(computations.Object, Light,
            computations.Point, computations.EyeVector, computations.NormalVector,
            isShadowed);
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
