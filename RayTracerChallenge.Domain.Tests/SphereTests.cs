using System;
using System.Linq;

namespace RayTracerChallenge.Domain.Tests;

public class SphereTests
{
    private readonly IFixture _fixture = new Fixture();

    private readonly TransformationFactory _factory = new TransformationFactory();

    private readonly IntersectionComparer _intersectionComparer = new IntersectionComparer();
    private readonly MatrixComparer _matrixComparer = new MatrixComparer();
    private readonly TupleComparer _tupleComparer = new TupleComparer();
    private readonly MaterialComparer _materialComparer = new MaterialComparer();

    [Fact]
    public void RayIntersectsSphereTwice()
    {
        var ray = new Ray(Tuple.CreatePoint(0, 0, -5), Tuple.CreateVector(0, 0, 1));
        var shape = new Sphere();

        var result = shape.Intersects(ray);

        var expectedResult = new[]
        {
            new Intersection(4.0M, shape),
            new Intersection(6.0M, shape)
        };
        Assert.Equal(expectedResult, result, _intersectionComparer);
    }

    [Fact]
    public void RayIntersectsSphereAtTangent()
    {
        var ray = new Ray(Tuple.CreatePoint(0, 1, -5), Tuple.CreateVector(0, 0, 1));
        var shape = new Sphere();

        var result = shape.Intersects(ray);

        var expectedResult = Enumerable.Repeat(new Intersection(5.0M, shape), 2);
        Assert.Equal(expectedResult, result, _intersectionComparer);
    }

    [Fact]
    public void RayMissedSphere()
    {
        var ray = new Ray(Tuple.CreatePoint(0, 2, -5), Tuple.CreateVector(0, 0, 1));
        var shape = new Sphere();

        var result = shape.Intersects(ray);

        Assert.Empty(result);
    }

    [Fact]
    public void RayStartsInsideSphere()
    {
        var ray = new Ray(Tuple.CreatePoint(0, 0, 0), Tuple.CreateVector(0, 0, 1));
        var shape = new Sphere();

        var result = shape.Intersects(ray);

        var expectedResult = new[]
        {
            new Intersection(-1.0M, shape),
            new Intersection(1.0M, shape)
        };
        Assert.Equal(expectedResult, result, _intersectionComparer);
    }

    [Fact]
    public void SphereIsBehindRay()
    {
        var ray = new Ray(Tuple.CreatePoint(0, 0, 5), Tuple.CreateVector(0, 0, 1));
        var shape = new Sphere();

        var result = shape.Intersects(ray);

        var expectedResult = new[]
        {
            new Intersection(-6.0M, shape),
            new Intersection(-4.0M, shape)
        };
        Assert.Equal(expectedResult, result, _intersectionComparer);
    }

    [Fact]
    public void SphereDefaultTransformation()
    {
        var shape = new Sphere();

        var expectedResult = Matrix.Identity();
        Assert.Equal(expectedResult, shape.Transform, _matrixComparer);
    }

    [Fact]
    public void IntersectingScaledSphereWithRay()
    {
        var ray = new Ray(Tuple.CreatePoint(0, 0, -5), Tuple.CreateVector(0, 0, 1));
        var shape = new Sphere()
        {
            Transform = _factory.Scale(2, 2, 2)
        };

        var result = shape.Intersects(ray);

        var expectedResult = new[]
        {
            new Intersection(3, shape),
            new Intersection(7, shape)
        };
        Assert.Equal(expectedResult, result, _intersectionComparer);
    }

    [Fact]
    public void NormalAtGivenVector()
    {
        var vector = Tuple.CreateVector(_fixture.Create<decimal>(),
            _fixture.Create<decimal>(),
            _fixture.Create<decimal>());

        var sphere = new Sphere();
        
        Assert.Throws<ArgumentException>(() => sphere.NormalAt(vector));
    }

    [Theory]
    [InlineData(1, 0, 0, 1, 0, 0)]
    [InlineData(0, 1, 0, 0, 1, 0)]
    [InlineData(0, 0, 1, 0, 0, 1)]
    [InlineData(0.5773502692, 0.5773502692, 0.5773502692, 0.5773502692, 0.5773502692, 0.5773502692)]
    public void NormalAt(decimal x, decimal y, decimal z,
        decimal expectedX, decimal expectedY, decimal expectedZ)
    {
        var sphere = new Sphere();
        var point = Tuple.CreatePoint(x, y, z);

        var result = sphere.NormalAt(point);

        var expectedResult = Tuple.CreateVector(expectedX, expectedY, expectedZ);
        Assert.Equal(expectedResult, result, _tupleComparer);
    }

    [Fact]
    public void NormalAtVectorIsNormalized()
    {
        var sphere = new Sphere();
        var point = Tuple.CreatePoint(0.5773502692M, 0.5773502692M, 0.5773502692M);

        var result = sphere.NormalAt(point);
        Assert.Equal(result, result.Normalize(), _tupleComparer);
    }

    [Fact]
    public void NormalOfTranslatedSphere()
    {
        var sphere = new Sphere()
        {
            Transform = _factory.Translation(0, 1, 0)
        };

        var result = sphere.NormalAt(Tuple.CreatePoint(0, 1.70711M, -0.70711M));

        var expectedResult = Tuple.CreateVector(0, 0.70711M, -0.70711M);
        Assert.Equal(expectedResult, result, _tupleComparer);
    }

    [Fact]
    public void NormalOfTransformedSphere()
    {
        var sphere = new Sphere()
        {
            Transform = _factory.Scale(1, 0.5M, 1) * _factory.RotationAroundZAxis(Math.PI / 5)
        };

        var result = sphere.NormalAt(Tuple.CreatePoint(0, Convert.ToDecimal(Math.Sqrt(2) / 2),
            -1 * Convert.ToDecimal(Math.Sqrt(2) / 2)));

        var expectedResult = Tuple.CreateVector(0, 0.97014M, -0.24254M);
        Assert.Equal(expectedResult, result, _tupleComparer);
    }
}
