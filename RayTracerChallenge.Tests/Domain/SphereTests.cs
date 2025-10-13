using RayTracerChallenge.Domain;
using RayTracerChallenge.Extensions;
using Tuple = RayTracerChallenge.Domain.Tuple;

namespace RayTracerChallenge.Tests.Domain;

public class SphereTests
{
    [Fact]
    public void RayIntersectsSphereTwice()
    {
        var ray = new Ray(Tuple.CreatePoint(0, 0, -5),
            Tuple.CreateVector(0, 0, 1));
        var s = new Sphere();

        var xs = s.Intersects(ray);
        var expectedResult = new Intersection[]
        {
            new(4, s),
            new(6, s)
        };

        Assert.Equal(expectedResult, xs);
    }

    [Fact]
    public void RayIntersectsSphereAtTangent()
    {
        var ray = new Ray(Tuple.CreatePoint(0, 1, -5),
            Tuple.CreateVector(0, 0, 1));
        var s = new Sphere();
        
        var xs = s.Intersects(ray);
        var expectedResult = new Intersection[]
        {
            new(5, s),
            new(5, s)
        };

        Assert.Equal(expectedResult, xs);
    }

    [Fact]
    public void RayMissesSphere()
    {
        var ray = new Ray(Tuple.CreatePoint(0, 2, -5),
            Tuple.CreateVector(0, 0, 1));
        var s = new Sphere();
        
        var xs = s.Intersects(ray);

        Assert.Empty(xs);
    }

    [Fact]
    public void RayOriginatesInsideSphere()
    {
        var ray = new Ray(Tuple.CreatePoint(0, 0, 0),
            Tuple.CreateVector(0, 0, 1));
        var s = new Sphere();
        
        var xs = s.Intersects(ray);
        var expectedResult = new Intersection[]
        {
            new(-1, s),
            new(1, s)
        };

        Assert.Equal(expectedResult, xs);
    }

    [Fact]
    public void SphereIsBehindRay()
    {
        var ray = new Ray(Tuple.CreatePoint(0, 0, 5),
            Tuple.CreateVector(0, 0, 1));
        var s = new Sphere();
        
        var xs = s.Intersects(ray);
        var expectedResult = new Intersection[]
        {
            new(-6, s),
            new(-4, s)
        };

        Assert.Equal(expectedResult, xs);
    }

    [Fact]
    public void IntersectSetsTheObjectOnTheIntersection()
    {
        var ray = new Ray(Tuple.CreatePoint(0, 0, -5),
            Tuple.CreateVector(0, 0, 1));
        var s = new Sphere();
        
        var xs = s.Intersects(ray);
        
        Assert.Equal(2, xs.Count);
        Assert.All(xs, (i) => Assert.Same(s, i.Sphere));
    }

    [Fact]
    public void SphereDefaultTransformation_IsIdentity()
    {
        var s = new Sphere();
        Assert.Same(Matrix.Identity, s.Transform);
    }

    [Fact]
    public void ChangeSphereTransformation()
    {
        var transform = Matrix.Identity
            .Translate(2, 3, 4);
        var s = new Sphere
        {
            Transform = transform
        };
        
        Assert.Same(transform, s.Transform);
    }

    [Fact]
    public void IntersectingScaledSphereWithRay()
    {
        var r = new Ray(Tuple.CreatePoint(0, 0, -5), 
            Tuple.CreateVector(0, 0, 1));
        var s = new Sphere
        {
            Transform = Matrix.Identity
                .Scale(2, 2, 2)
        };
        
        var xs = s.Intersects(r);
        var expectedResult = new Intersection[]
        {
            new(3, s),
            new(7, s)
        };
        
        Assert.Equal(expectedResult, xs);
    }

    [Fact]
    public void IntersectingTransformedSphereWithRay()
    {
        var r = new Ray(Tuple.CreatePoint(0, 0, -5), 
            Tuple.CreateVector(0, 0, 1));
        var s = new Sphere
        {
            Transform = Matrix.Identity
                .Translate(5, 0, 0)
        };
        
        var xs = s.Intersects(r);
        
        Assert.Empty(xs);
    }
}