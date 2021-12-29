using Moq.Protected;
using RayTracerChallenge.Domain.Extensions;

namespace RayTracerChallenge.Domain.Tests;

public class IntersectionTests
{
    private readonly TransformationFactory _factory = new TransformationFactory();

    private readonly TupleComparer _tupleComparer = new TupleComparer();

    [Fact]
    public void IntersectionEncapsulatesTAndObject()
    {
        var shape = new TestShape();
        var t = 3.5M;

        var intersection = new Intersection(t, shape);

        Assert.Equal(t, intersection.T);
        Assert.Same(shape, intersection.Object);
    }

    [Fact]
    public void FindHitInIntersectionsWithPositiveT()
    {
        var shape = new TestShape();
        var intersection1 = new Intersection(1, shape);
        var intersection2 = new Intersection(2, shape);
        var intersections = new[] { intersection1, intersection2 };

        var result = intersections.Hit();

        Assert.Same(intersection1, result);
    }

    [Fact]
    public void FindHitInIntersectionsWithSomeNegativeT()
    {
        var shape = new TestShape();
        var intersection1 = new Intersection(-1, shape);
        var intersection2 = new Intersection(1, shape);
        var intersections = new[] { intersection1, intersection2 };

        var result = intersections.Hit();

        Assert.Same(intersection2, result);
    }

    [Fact]
    public void NoHitInIntersections()
    {
        var shape = new TestShape();
        var intersection1 = new Intersection(-2, shape);
        var intersection2 = new Intersection(-1, shape);
        var intersections = new[] { intersection1, intersection2 };

        var result = intersections.Hit();

        Assert.Null(result);
    }

    [Fact]
    public void HitIsLowestNonNegativeIntersection()
    {
        var shape = new TestShape();
        var intersection1 = new Intersection(5, shape);
        var intersection2 = new Intersection(7, shape);
        var intersection3 = new Intersection(-3, shape);
        var intersection4 = new Intersection(2, shape);
        var intersections = new[]
        {
            intersection1,
            intersection2,
            intersection3,
            intersection4
        };

        var result = intersections.Hit();

        Assert.Same(intersection4, result);
    }

    [Fact]
    public void CheckPreparedComputations()
    {
        var ray = new Ray(Tuple.CreatePoint(0, 0, -5), Tuple.CreateVector(0, 0, 1));
        var mockShape = new Mock<Shape>();
        var intersection = new Intersection(4, mockShape.Object);

        var expectedNormal = Tuple.CreateVector(0, 0, -1);
        mockShape.Protected()
            .Setup<Tuple>("LocalNormalAt", ItExpr.IsAny<Tuple>())
            .Returns(expectedNormal);

        var result = intersection.PrepareComputations(ray);

        Assert.Equal(intersection.T, result.T);
        Assert.Same(intersection.Object, result.Object);
        
        var expectedPoint = Tuple.CreatePoint(0, 0, -1);
        Assert.Equal(expectedPoint, result.Point, _tupleComparer);

        var expectedEye = Tuple.CreateVector(0, 0, -1);
        Assert.Equal(expectedEye, result.EyeVector, _tupleComparer);

        Assert.Equal(expectedNormal, result.NormalVector, _tupleComparer);

        Assert.False(result.Inside);
    }

    [Fact]
    public void IntersectionOccursInside()
    {
        var ray = new Ray(Tuple.CreatePoint(0, 0, 0), Tuple.CreateVector(0, 0, 1));
        var sphere = new Sphere();
        var intersection = new Intersection(1, sphere);

        var result = intersection.PrepareComputations(ray);

        var expectedPoint = Tuple.CreatePoint(0, 0, 1);
        Assert.Equal(expectedPoint, result.Point, _tupleComparer);

        var expectedEye = Tuple.CreateVector(0, 0, -1);
        Assert.Equal(expectedEye, result.EyeVector, _tupleComparer);

        var expectedNormal = Tuple.CreateVector(0, 0, -1);
        Assert.Equal(expectedNormal, result.NormalVector, _tupleComparer);

        Assert.True(result.Inside);
    }

    [Fact]
    public void HitShouldOffsetPoint()
    {
        var ray = new Ray(Tuple.CreatePoint(0, 0, -5), Tuple.CreateVector(0, 0, 1));
        var shape = new Sphere
        {
            Transform = _factory.Translation(0, 0, 1)
        };
        var intersection = new Intersection(5, shape);
        var computations = intersection.PrepareComputations(ray);

        Assert.True(computations.OverPoint.Z < -0.0001M / 2);
        Assert.True(computations.Point.Z > computations.OverPoint.Z);
    }
}
