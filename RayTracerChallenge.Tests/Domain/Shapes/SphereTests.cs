using RayTracerChallenge.Domain;
using RayTracerChallenge.Domain.Shapes;
using RayTracerChallenge.Extensions;
using Tuple = RayTracerChallenge.Domain.Tuple;

namespace RayTracerChallenge.Tests.Domain.Shapes;

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
        Assert.All(xs, (i) => Assert.Same(s, i.Shape));
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
        var s = new Sphere(transform);
        
        Assert.Same(transform, s.Transform);
    }

    [Fact]
    public void IntersectingScaledSphereWithRay()
    {
        var r = new Ray(Tuple.CreatePoint(0, 0, -5), 
            Tuple.CreateVector(0, 0, 1));
        var s = new Sphere(Matrix.Identity
                .Scale(2, 2, 2));
        
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
        var s = new Sphere(Matrix.Identity
                .Translate(5, 0, 0));
        
        var xs = s.Intersects(r);
        
        Assert.Empty(xs);
    }

    [Theory, MemberData((nameof(NormalOnSphereData)))]
    public void NormalOnSphere(double x, double y, double z,
        double normalX, double normalY, double normalZ)
    {
        var s = new Sphere();
        
        var n = s.NormalAt(Tuple.CreatePoint(x, y, z));
        var expectedResult = Tuple.CreateVector(normalX, normalY, normalZ);
        
        Assert.Equal(expectedResult, n);
    }

    public static TheoryData<double, double, double, double, double, double> NormalOnSphereData => new()
    {
        { 1, 0, 0, 1, 0, 0 },
        { 0, 1, 0, 0, 1, 0 },
        { 0, 0, 1, 0, 0, 1 },
        {
            0.5773502692, 0.5773502692, 0.5773502692,
            0.5773502692, 0.5773502692, 0.5773502692
        }
    };

    [Fact]
    public void NormalIsNormalizedVector()
    {
        var s = new Sphere();
        
        var n = s.NormalAt(Tuple.CreatePoint(0.5773502692,
            0.5773502692, 0.5773502692));
        var expectedResult = n.Normalize();
        
        Assert.Equal(expectedResult, n);
    }

    [Fact]
    public void ComputeNormalOfTranslatedSphere()
    {
        var s = new Sphere(Matrix.Identity.Translate(0, 1, 0));
        
        var n = s.NormalAt(Tuple.CreatePoint(0, 1.70711, -0.70711));
        var expectedResult = Tuple.CreateVector(0, 0.70711, -0.70711);
        
        Assert.Equal(expectedResult, n);
    }

    [Fact]
    public void ComputeNormalOfTransformedSphere()
    {
        var s = new Sphere(Matrix.Identity
                .RotateZ(Math.PI / 5)
                .Scale(1, 0.5, 1));
        
        var n = s.NormalAt(Tuple.CreatePoint(0,
            Math.Sqrt(2)/2,
            -Math.Sqrt(2)/2));
        var expectedResult = Tuple.CreateVector(0, 0.97014, -0.24254);
        
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

    [Fact]
    public void GlassSphereIsTransparent()
    {
        var s = Sphere.Glass();
        
        Assert.Equal(Matrix.Identity, s.Transform);
        Assert.Equal(1, s.Material.Transparency);
        Assert.Equal(1.5, s.Material.RefractiveIndex);
    }

    [Fact]
    public void ShapeHasParent()
    {
        var s = new Sphere();
        Assert.Null(s.Parent);
    }

    [Fact]
    public void BoundsOfASphere()
    {
        var s = new Sphere();
        
        var result = s.GetBounds();
        var expected = new Bounds(-1, -1, -1, 1, 1, 1);
        
        Assert.Equal(expected, result);
    }
}