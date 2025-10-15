using RayTracerChallenge.Domain;
using RayTracerChallenge.Extensions;
using Tuple = RayTracerChallenge.Domain.Tuple;

namespace RayTracerChallenge.Tests.Domain;

public class WorldTests
{
    [Fact]
    public void EmptyWorld()
    {
        var w = new World();
        
        Assert.Empty(w.Objects);
        Assert.Null(w.Light);
    }

    [Fact]
    public void DefaultWorld()
    {
        var w = World.DefaultWorld();

        var expectedLight = new PointLight(Tuple.CreatePoint(-10, 10, -10),
            new Color(1, 1, 1));
        var expectedObjects = new[]
        {
            new Sphere
            {
                Material = new Material
                {
                    Color = new Color(0.8, 1.0, 0.6),
                    Diffuse = 0.7,
                    Specular = 0.2,
                }
            },
            new Sphere(Matrix.Identity
                .Scale(0.5, 0.5, 0.5))
        };
        
        Assert.Equal(expectedLight, w.Light);
        Assert.Equal(expectedObjects, w.Objects);
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
        var i = new Intersection(4, w.Objects.First());

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
        var i = new Intersection(0.5, w.Objects.ElementAt(1));
        
        var comps = i.PrepareComputations(r);
        var result = w.ShadeHit(comps);
        var expectedResult = new Color(0.90498, 0.90498, 0.90498);
        
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
        w.Objects.Add(outer);
        var inner = new Sphere(Matrix.Identity
            .Scale(0.5, 0.5, 0.5))
        {
            Material = new Material
            {
                Ambient = 1
            }
        };
        w.Objects.Add(inner);

        var r = new Ray(Tuple.CreatePoint(0, 0, 0.75),
            Tuple.CreateVector(0, 0, -1));
        
        var result = w.ColorAt(r);
        
        Assert.Equal(inner.Material.Color, result);
    }
}