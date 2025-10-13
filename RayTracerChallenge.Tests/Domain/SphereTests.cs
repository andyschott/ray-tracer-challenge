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

    [Theory, MemberData((nameof(NormalOnSphereData)))]
    public void NormalOnSphere(decimal x, decimal y, decimal z,
        decimal normalX, decimal normalY, decimal normalZ)
    {
        var s = new Sphere();
        
        var n = s.NormalAt(Tuple.CreatePoint(x, y, z));
        var expectedResult = Tuple.CreateVector(normalX, normalY, normalZ);
        
        Assert.Equal(expectedResult, n);
    }

    public static TheoryData<decimal, decimal, decimal, decimal, decimal, decimal> NormalOnSphereData => new()
    {
        { 1, 0, 0, 1, 0, 0 },
        { 0, 1, 0, 0, 1, 0 },
        { 0, 0, 1, 0, 0, 1 },
        {
            0.5773502692M, 0.5773502692M, 0.5773502692M,
            0.5773502692M, 0.5773502692M, 0.5773502692M
        }
    };

    [Fact]
    public void NormalIsNormalizedVector()
    {
        var s = new Sphere();
        
        var n = s.NormalAt(Tuple.CreatePoint(0.5773502692M,
            0.5773502692M, 0.5773502692M));
        var expectedResult = n.Normalize();
        
        Assert.Equal(expectedResult, n);
    }

    [Fact]
    public void ComputeNormalOfTranslatedSphere()
    {
        var s = new Sphere
        {
            Transform = Matrix.Identity.Translate(0, 1, 0)
        };
        
        var n = s.NormalAt(Tuple.CreatePoint(0, 1.70711M, -0.70711M));
        var expectedResult = Tuple.CreateVector(0, 0.70711M, -0.70711M);
        
        Assert.Equal(expectedResult, n);
    }

    [Fact]
    public void ComputeNormalOfTransformedSphere()
    {
        var s = new Sphere
        {
            Transform = Matrix.Identity
                .RotateZ(Math.PI / 5)
                .Scale(1, 0.5M, 1)
        };
        
        var n = s.NormalAt(Tuple.CreatePoint(0,
            (decimal)Math.Sqrt(2)/2,
            -(decimal)Math.Sqrt(2)/2));
        var expectedResult = Tuple.CreateVector(0, 0.97014M, -0.24254M);
        
        Assert.Equal(expectedResult, n);
    }

    [Fact]
    public void SphereHasDefaultMaterial()
    {
        var s = new Sphere();
        
        Assert.Equal(new Material(), s.Material);
    }

    [Fact]
    public void SphereCanBeAssignedMaterial()
    {
        var m = new Material(ambient: 1);
        var s = new Sphere
        {
            Material = m
        };
        
        Assert.Same(m, s.Material);
    }
}