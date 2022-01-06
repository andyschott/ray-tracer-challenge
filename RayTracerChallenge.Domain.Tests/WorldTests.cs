using System;
using System.Linq;
using AutoFixture.Kernel;
using RayTracerChallenge.Domain.Patterns;
using RayTracerChallenge.Domain.Tests.Extensions;

namespace RayTracerChallenge.Domain.Tests;

public class WorldTests
{
    private readonly IFixture _fixture = new Fixture();

    private readonly TransformationFactory _factory = new TransformationFactory();

    private readonly LightComparer _lightComparer = new LightComparer();
    private readonly ColorComparer _colorComparer = new ColorComparer();

    public WorldTests()
    {
        _fixture.Register(() => new IntersectionComputations(_fixture.Create<decimal>(),
            _fixture.Create<Shape>(), _fixture.CreatePoint(),
            _fixture.CreateVector(), _fixture.CreateVector(),
            _fixture.CreateVector()));

        _fixture.Customizations.Add(new TypeRelay(typeof(Shape), typeof(TestShape)));
        _fixture.Customize<Material>(c => c.Without(m => m.Pattern));
    }

    [Fact]
    public void EmptyWorld()
    {
        var world = new World();

        Assert.Null(world.Light);
        Assert.Empty(world.Objects);
    }

    [Fact]
    public void VerifyDefaultWorld()
    {
        var world = World.Default();

        var expectedLight = new Light(Tuple.CreatePoint(-10, 10, -10),
            new Color
            {
                Red = 1,
                Green = 1,
                Blue = 1
            });

        Assert.NotNull(world.Light);
        if(world.Light is not null) // to make the compiler happy
        {
            Assert.Equal(expectedLight, world.Light, _lightComparer);
        }

        Assert.Equal(2, world.Objects.Count());
    }

    [Fact]
    public void IntersectWorldWithRay()
    {
        var world = World.Default();
        var ray = new Ray(Tuple.CreatePoint(0, 0, -5), Tuple.CreateVector(0, 0, 1));

        var intersections = world.Intersect(ray)
            .ToArray();

        Assert.Equal(4, intersections.Length);
        Assert.Equal(4, intersections[0].T);
        Assert.Equal(4.5M, intersections[1].T);
        Assert.Equal(5.5M, intersections[2].T);
        Assert.Equal(6, intersections[3].T);
    }

    [Fact]
    public void ShadingAnIntersection()
    {
        var world = World.Default();
        var ray = new Ray(Tuple.CreatePoint(0, 0, -5), Tuple.CreateVector(0, 0, 1));
        var intersection = new Intersection(4, world.Objects.First());

        var computations = intersection.PrepareComputations(ray);
        var result = world.ShadeHit(computations);

        var expectedResult = new Color
        {
            Red = 0.38066M,
            Green = 0.47583M,
            Blue = 0.2855M
        };

        Assert.Equal(expectedResult, result, _colorComparer);
    }

    [Fact]
    public void ShadeInsideIntersection()
    {
        var world = World.Default(new Light(Tuple.CreatePoint(0, 0.25M, 0), new Color
        {
            Red = 1,
            Green = 1,
            Blue = 1
        }));
        var ray = new Ray(Tuple.CreatePoint(0, 0, 0), Tuple.CreateVector(0, 0, 1));
        var intersection = new Intersection(0.5M, world.Objects.ElementAt(1));

        var computations = intersection.PrepareComputations(ray);
        var result = world.ShadeHit(computations);

        var expectedResult = new Color
        {
            Red = 0.90498M,
            Green = 0.90498M,
            Blue = 0.90498M
        };

        Assert.Equal(expectedResult, result, _colorComparer);
    }

    [Fact]
    public void ShadeHitWithNoLight()
    {
        var world = new World();
        var computations = _fixture.Create<IntersectionComputations>();

        Assert.Throws<NullReferenceException>(() => world.ShadeHit(computations));
    }

    [Fact]
    public void ColorWhenRayMisses()
    {
        var world = World.Default();
        var ray = new Ray(Tuple.CreatePoint(0, 0, -5), Tuple.CreateVector(0, 1, 0));

        var result = world.ColorAt(ray);

        Assert.Equal(Color.Black, result, _colorComparer);
    }

    [Fact]
    public void ColorWhenRayHits()
    {
        var world = World.Default();
        var ray = new Ray(Tuple.CreatePoint(0, 0, -5), Tuple.CreateVector(0, 0, 1));

        var result = world.ColorAt(ray);

        var expectedResult = new Color
        {
            Red = 0.38066M,
            Green = 0.47583M,
            Blue = 0.2855M
        };
        Assert.Equal(expectedResult, result, _colorComparer);
    }

    [Fact]
    public void ColorWhenIntersectionBehindRay()
    {
        var world = World.Default(null, new Material
        {
                Ambient = 1,
                Diffuse = 0.7M,
                Specular = 0.2M,
                Pattern = new SolidPattern(new Color
                {
                    Red = 0.8M,
                    Green = 1.0M,
                    Blue = 0.6M
                })
            }, new Material
            {
                Ambient = 1
            });

        var ray = new Ray(Tuple.CreatePoint(0, 0, 0.75M), Tuple.CreateVector(0, 0, -1));

        var result = world.ColorAt(ray);

        var solidPattern = (SolidPattern)world.Objects.ElementAt(1).Material.Pattern;

        Assert.Equal(solidPattern.Color, result, _colorComparer);
    }

    [Fact]
    public void IsShadowedMustTakePoint()
    {
        var world = _fixture.Build<World>()
            .With(world => world.Light, _fixture.CreateLight())
            .Create();
        var vector = _fixture.CreateVector();

        Assert.Throws<ArgumentException>(() => world.IsShadowed(vector));
    }

    [Fact]
    public void NoShadows()
    {
        var world = World.Default();
        var position = Tuple.CreatePoint(0, 10, 0);

        var result = world.IsShadowed(position);

        Assert.False(result);
    }

    [Fact]
    public void NoShadowWithoutLight()
    {
        var world = _fixture.Build<World>()
            .Without(world => world.Light)
            .Create();
        var position = _fixture.CreatePoint();

        var result = world.IsShadowed(position);

        Assert.False(result);
    }

    [Fact]
    public void NoShadowWhenBehindLight()
    {
        var world = World.Default();
        var position = Tuple.CreatePoint(-20, 20, -20);

        var result = world.IsShadowed(position);

        Assert.False(result);
    }

    [Fact]
    public void NoShadowWhenObjectBehindPoint()
    {
        var world = World.Default();
        var position = Tuple.CreatePoint(-2, 2, -2);

        var result = world.IsShadowed(position);

        Assert.False(result);
    }

    [Fact]
    public void IntersectionWithShadow()
    {
        var world = new World
        {
            Light = new Light(Tuple.CreatePoint(0, 0, -10), Color.White),
            Objects = new[]
            {
                new Sphere(),
                new Sphere
                {
                    Transform = _factory.Translation(0, 0, 10)
                }
            }
        };
        var ray = new Ray(Tuple.CreatePoint(0, 0, 5), Tuple.CreateVector(0, 0, 1));
        var intersection = new Intersection(4, world.Objects.ElementAt(1));
        var computations = intersection.PrepareComputations(ray);

        var result = world.ShadeHit(computations);

        var expectedResult = new Color
        {
            Red = 0.1M,
            Green = 0.1M,
            Blue = 0.1M
        };
        Assert.Equal(expectedResult, result, _colorComparer);
    }

    [Fact]
    public void ReflectedColorOfNonReflectiveMaterial()
    {
        var world = World.Default(material2: new Material
        {
            Ambient = 1
        });
        var ray = new Ray(Tuple.CreatePoint(0, 0, 0), Tuple.CreateVector(0, 0, 1));
        var intersection = new Intersection(1, world.Objects.ElementAt(1));
        var computations = intersection.PrepareComputations(ray);

        var result = world.ReflectedColor(computations);

        Assert.Equal(Color.Black, result, _colorComparer);
    }

    [Fact]
    public void ReflectedColorOfReflectiveMaterial()
    {
        var world = World.Default();
        var shape = new Plane
        {
            Material = new Material
            {
                Reflective = 0.5M
            },
            Transform = _factory.Translation(0, -1, 0)
        };
        world.Objects.Add(shape);

        var ray = new Ray(Tuple.CreatePoint(0, 0, -3),
            Tuple.CreateVector(0, Convert.ToDecimal(-1 * Math.Sqrt(2) / 2), Convert.ToDecimal(Math.Sqrt(2) / 2)));
        var intersection = new Intersection(Convert.ToDecimal(Math.Sqrt(2)), shape);
        var computations = intersection.PrepareComputations(ray);

        var result = world.ReflectedColor(computations);

        var expectedResult = new Color
        {
            Red = 0.19032M,
            Green = 0.2379M,
            Blue = 0.14274M
        };
        Assert.Equal(expectedResult, result, _colorComparer);
    }

    [Fact]
    public void ShadeHitWithReflectiveMaterial()
    {
        var world = World.Default();
        var shape = new Plane
        {
            Material = new Material
            {
                Reflective = 0.5M
            },
            Transform = _factory.Translation(0, -1, 0)
        };
        world.Objects.Add(shape);

        var ray = new Ray(Tuple.CreatePoint(0, 0, -3),
            Tuple.CreateVector(0, Convert.ToDecimal(-1 * Math.Sqrt(2) / 2), Convert.ToDecimal(Math.Sqrt(2) / 2)));
        var intersection = new Intersection(Convert.ToDecimal(Math.Sqrt(2)), shape);
        var computations = intersection.PrepareComputations(ray);

        var result = world.ShadeHit(computations);

        var expectedResult = new Color
        {
            Red = 0.87677M,
            Green = 0.92436M,
            Blue = 0.82918M
        };
        Assert.Equal(expectedResult, result, _colorComparer);
    }
}
