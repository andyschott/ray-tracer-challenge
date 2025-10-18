using RayTracerChallenge.Domain;
using RayTracerChallenge.Domain.Shapes;
using RayTracerChallenge.Extensions;
using Tuple = RayTracerChallenge.Domain.Tuple;

namespace RayTracerChallenge.Tests.Domain;

public class WorldTests
{
    [Fact]
    public void EmptyWorld()
    {
        var w = new World();
        
        Assert.Empty(w.Shapes);
        Assert.Null(w.Light);
    }

    [Fact]
    public void DefaultWorld()
    {
        var w = World.DefaultWorld();

        var expectedLight = new PointLight(Tuple.CreatePoint(-10, 10, -10),
            new Color(1, 1, 1));
        
        Assert.Equal(expectedLight, w.Light);
        Assert.Equal(2, w.Shapes.Count);
    }

    [Fact]
    public void IntersectWorldWithRay()
    {
        var w = World.DefaultWorld();
        var r = new Ray(Tuple.CreatePoint(0, 0, -5),
            Tuple.CreateVector(0, 0, 1));
        
        var xs = w.Intersect(r);
        
        Assert.Equal(4, xs.Count);
        Assert.Equal(4, xs[0].T);
        Assert.Equal(4.5, xs[1].T);
        Assert.Equal(5.5, xs[2].T);
        Assert.Equal(6, xs[3].T);
    }

    [Fact]
    public void ShadingAnIntersection()
    {
        var w = World.DefaultWorld();
        var r = new Ray(Tuple.CreatePoint(0, 0, -5),
            Tuple.CreateVector(0, 0, 1));
        var i = new Intersection(4, w.Shapes.First());

        var comps = i.PrepareComputations(r);
        var result = w.ShadeHit(comps);
        var expectedResult = new Color(0.38066, 0.47583, 0.2855);
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void ShadingAnIntersectionFromInside()
    {
        var w = World.DefaultWorld();
        w.Light = new PointLight(Tuple.CreatePoint(0, 0.25, 0), 
            new Color(1, 1, 1));
        var r = new Ray(Tuple.CreatePoint(0, 0, 0),
            Tuple.CreateVector(0, 0, 1));
        var i = new Intersection(0.5, w.Shapes.ElementAt(1));
        
        var comps = i.PrepareComputations(r);
        var result = w.ShadeHit(comps);
        var expectedResult = new Color(0.90498, 0.90498, 0.90498);
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void ShadingWhenIntersectionIsInShadow()
    {
        var w = new World
        {
            Light = new PointLight(Tuple.CreatePoint(0, 0, -10),
                new Color(1, 1, 1))
        };

        var s1 = new Sphere();
        w.Shapes.Add(s1);
        
        var s2 = new Sphere(Matrix.Identity.Translate(0, 0, 10));
        w.Shapes.Add(s2);
        
        var r = new Ray(Tuple.CreatePoint(0, 0, 5),
            Tuple.CreateVector(0, 0, 1));
        var i = new Intersection(4, s2);
        var comps = i.PrepareComputations(r);
        
        var result = w.ShadeHit(comps);
        var expectedResult = new Color(0.1, 0.1, 0.1);
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void ColorWhenRayMisses()
    {
        var w = World.DefaultWorld();
        var r = new Ray(Tuple.CreatePoint(0, 0, -5),
            Tuple.CreateVector(0, 1, 0));
        
        var result = w.ColorAt(r);
        var expectedResult = new Color(0, 0, 0);
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void ColorWhenRayHits()
    {
        var w = World.DefaultWorld();
        var r = new Ray(Tuple.CreatePoint(0, 0, -5),
            Tuple.CreateVector(0, 0, 1));
        
        var result = w.ColorAt(r);
        var expectedResult = new Color(0.38066, 0.47583, 0.2855);
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void ColorWithIntersectionBehindRay()
    {
        var w = new World
        {
            Light = World.DefaultLight
        };
        var outer = new Sphere
        {
            Material = new Material
            {
                Color = new Color(0.8, 1.0, 0.6),
                Ambient = 1,
                Diffuse = 0.7,
                Specular = 0.2,
            }
        };
        w.Shapes.Add(outer);
        var inner = new Sphere(Matrix.Identity
            .Scale(0.5, 0.5, 0.5))
        {
            Material = new Material
            {
                Ambient = 1
            }
        };
        w.Shapes.Add(inner);

        var r = new Ray(Tuple.CreatePoint(0, 0, 0.75),
            Tuple.CreateVector(0, 0, -1));
        
        var result = w.ColorAt(r);
        
        Assert.Equal(inner.Material.Color, result);
    }

    [Theory]
    [InlineData(0, 10, 0, false)]
    [InlineData(10, -10, 10, true)]
    [InlineData(-20, 20, -20, false)]
    [InlineData(-2, 2, -2, false)]
    public void IsShadowed_TestCases(double x,
        double y,
        double z,
        bool expectedResult)
    {
        var w = World.DefaultWorld();
        var p = Tuple.CreatePoint(x, y, z);

        var result = w.IsShadowed(p);
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void ReflectedColorOfNonReflectiveMaterial()
    {
        var w = World.DefaultWorld(shape2Material: new Material
        {
            Ambient = 1,
        });
        var r = new Ray(0, 0, 0, 0, 0, 1);
        var i = new Intersection(1, w.Shapes[1]);
        
        var comps = i.PrepareComputations(r);
        var result = w.ReflectedColor(comps);
        var expectedResult = new Color(0, 0, 0);
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void ReflectedColorForReflectiveMaterial()
    {
        var w = World.DefaultWorld();
        var shape = new Plane(Matrix.Identity.Translate(0, -1, 0))
        {
            Material = new Material
            {
                Reflective = 0.5
            }
        };
        w.Shapes.Add(shape);
        
        var r = new Ray(0, 0, -3, 0, -1 * Math.Sqrt(2)/2, Math.Sqrt(2)/2);
        var i = new Intersection(Math.Sqrt(2), shape);
        var comps = i.PrepareComputations(r);
        
        var result = w.ReflectedColor(comps);
        var expectedResult = new Color(0.19032, 0.2379, 0.14274);
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void ShadeHitWithReflectiveMaterial()
    {
        var w = World.DefaultWorld();
        var shape = new Plane(Matrix.Identity.Translate(0, -1, 0))
        {
            Material = new Material
            {
                Reflective = 0.5
            }
        };
        w.Shapes.Add(shape);
        
        var r = new Ray(0, 0, -3, 0, -1 * Math.Sqrt(2)/2, Math.Sqrt(2)/2);
        var i = new Intersection(Math.Sqrt(2), shape);
        var comps = i.PrepareComputations(r);
        
        var result = w.ShadeHit(comps);
        var expectedResult = new Color(0.87677, 0.92436, 0.82918);
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void ColorAtWithMutuallyReflectiveSurfaces()
    {
        var w = World.DefaultWorld();
        w.Light = new PointLight(0, 0, 0, 1, 1, 1);

        var lower = new Plane(Matrix.Identity.Translate(0, -1, 0))
        {
            Material = new Material
            {
                Reflective = 1
            }
        };
        w.Shapes.Add(lower);

        var upper = new Plane(Matrix.Identity.Translate(0, 1, 0))
        {
            Material = new Material
            {
                Reflective = 1
            }
        };
        w.Shapes.Add(upper);

        var r = new Ray(0, 0, 0, 0, 1, 0);
        
        var exception = Record.Exception(() => w.ColorAt(r));
        Assert.Null(exception);
    }

    [Fact]
    public void ReflectedColorAtMaxRecursiveDepth()
    {
        var w = World.DefaultWorld();
        var shape = new Plane(Matrix.Identity.Translate(0, -1, 0))
        {
            Material = new Material
            {
                Reflective = 0.5
            }
        };
        w.Shapes.Add(shape);
        
        var r = new Ray(0, 0, -3, 0, -1 * Math.Sqrt(2)/2, Math.Sqrt(2)/2);
        var i = new Intersection(Math.Sqrt(2), shape);
        var comps = i.PrepareComputations(r);
        
        var result = w.ReflectedColor(comps, 0);
        var expectedResult = new Color(0, 0, 0);
        
        Assert.Equal(expectedResult, result);        
    }

    [Fact]
    public void RefractedColorOfOpaqueSurface()
    {
        var w = World.DefaultWorld();
        var shape = w.Shapes[0];
        var r = new Ray(0, 0, -5, 0, 0, 1);
        var xs = new Intersections
        {
            new Intersection(4, shape),
            new Intersection(6, shape),
        };
        var comps = xs[0].PrepareComputations(r, xs);

        var result = w.RefractedColor(comps);
        var expectedResult = new Color(0, 0, 0);
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void RefractedColorAtMaxRecursiveDepth()
    {
        var w = World.DefaultWorld(new Material
        {
            Transparency = 1,
            RefractiveIndex = 1.5
        });
        var shape = w.Shapes[0];
        var r = new Ray(0, 0, -5, 0, 0, 1);
        var xs = new Intersections
        {
            new Intersection(4, shape),
            new Intersection(6, shape),
        };
        var comps = xs[0].PrepareComputations(r, xs);
        
        var result = w.RefractedColor(comps, 0);
        var expectedResult = new Color(0, 0, 0);
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void RefractedColorUnderTotalInternalReflection()
    {
        var w = World.DefaultWorld(new Material
        {
            Transparency = 1,
            RefractiveIndex = 1.5
        });
        var shape = w.Shapes[0];
        var r = new Ray(0, 0, Math.Sqrt(2) / 2, 0, 1, 0);
        var xs = new Intersections
        {
            new Intersection(-1 * Math.Sqrt(2) / 2, shape),
            new Intersection(Math.Sqrt(2) / 2, shape)
        };
        var comps = xs[1].PrepareComputations(r, xs);

        var result = w.RefractedColor(comps);
        var expectedResult = new Color(0, 0, 0);
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void RefractedColorWithRefractedRay()
    {
        var w = World.DefaultWorld(new Material
        {
            Ambient = 1,
            Pattern = new TestPattern()
        }, new Material
        {
            Transparency = 1,
            RefractiveIndex = 1.5
        });
        var r = new Ray(0, 0, 0.1, 0, 1, 0);
        var xs = new Intersections
        {
            new Intersection(-0.9899, w.Shapes[0]),
            new Intersection(-0.4899, w.Shapes[1]),
            new Intersection(0.4899, w.Shapes[1]),
            new Intersection(0.9899, w.Shapes[0])
        };
        var comps = xs[2].PrepareComputations(r, xs);
        
        var result = w.RefractedColor(comps);
        var expectedResult = new Color(0, 0.99888, 0.04725);
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void ShadeHitWithTransparentMaterial()
    {
        var w = World.DefaultWorld();
        var floor = new Plane(Matrix.Identity.Translate(0, -1, 0))
        {
            Material = new Material
            {
                Transparency = 0.5,
                RefractiveIndex = 1.5
            }
        };
        w.Shapes.Add(floor);

        var ball = new Sphere(Matrix.Identity.Translate(0, -3.5, -0.5))
        {
            Material = new Material
            {
                Color = new Color(1, 0, 0),
                Ambient = 0.5,
            }
        };
        w.Shapes.Add(ball);
        
        var r = new Ray(0, 0, -3, 0, -1 * Math.Sqrt(2)/2, Math.Sqrt(2)/2);
        var i = new Intersection(Math.Sqrt(2), floor);
        var comps = i.PrepareComputations(r);

        var result = w.ShadeHit(comps);
        var expectedResult = new Color(0.93642, 0.68642, 0.68642);
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void ShadeHitWithReflectiveTransparentMaterial()
    {
        var w = World.DefaultWorld();
        var floor = new Plane(Matrix.Identity.Translate(0, -1, 0))
        {
            Material = new Material
            {
                Reflective = 0.5,
                Transparency = 0.5,
                RefractiveIndex = 1.5
            }
        };
        w.Shapes.Add(floor);

        var ball = new Sphere(Matrix.Identity.Translate(0, -3.5, -0.5))
        {
            Material = new Material
            {
                Color = new Color(1, 0, 0),
                Ambient = 0.5
            }
        };
        w.Shapes.Add(ball);
        
        var r = new Ray(0, 0, -3, 0, -1 * Math.Sqrt(2)/2, Math.Sqrt(2)/2);
        var i = new Intersection(Math.Sqrt(2), floor);
        var comps = i.PrepareComputations(r);
        
        var result = w.ShadeHit(comps);
        var expectedResult = new Color(0.93391, 0.69643, 0.69243);
        
        Assert.Equal(expectedResult, result);
    }
}