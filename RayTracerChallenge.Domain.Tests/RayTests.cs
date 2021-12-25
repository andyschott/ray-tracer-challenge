using System;

namespace RayTracerChallenge.Domain.Tests;

public class RayTests
{
    private readonly TransformationFactory _factory = new TransformationFactory();
    private readonly TupleComparer _tupleComparer = new TupleComparer();
    private readonly RayComparer _rayComparer = new RayComparer();

    [Fact]
    public void CreateRay()
    {
        var origin = Tuple.CreatePoint(1, 2, 3);
        var direction = Tuple.CreateVector(4, 5, 6);

        var ray = new Ray(origin, direction);

        var expectedResult = new Ray(origin, direction);
        Assert.Equal(expectedResult, ray, _rayComparer);
    }

    [Fact]
    public void InvalidOrigin()
    {
        var origin = Tuple.CreateVector(1, 2, 3);
        var direction = Tuple.CreateVector(4, 5, 6);

        Assert.Throws<ArgumentException>(() => new Ray(origin, direction));
    }

    [Fact]
    public void InvalidDirection()
    {
        var origin = Tuple.CreatePoint(1, 2, 3);
        var direction = Tuple.CreatePoint(4, 5, 6);

        Assert.Throws<ArgumentException>(() => new Ray(origin, direction));
    }

    [Theory]
    [InlineData(0, 2, 3, 4)]
    [InlineData(1, 3, 3, 4)]
    [InlineData(-1, 1, 3, 4)]
    [InlineData(2.5, 4.5, 3, 4)]
    public void ComputePointFromDistance(decimal t, decimal expectedX, decimal expectedY, decimal expectedZ)
    {
        var ray = new Ray(Tuple.CreatePoint(2, 3, 4), Tuple.CreateVector(1, 0, 0));

        var position = ray.Position(t);

        var expectedResult = Tuple.CreatePoint(expectedX, expectedY, expectedZ);
        Assert.Equal(expectedResult, position, _tupleComparer);
    }

    [Fact]
    public void TranslateRay()
    {
        var ray = new Ray(Tuple.CreatePoint(1, 2, 3), Tuple.CreateVector(0, 1, 0));
        var translation = _factory.Translation(3, 4, 5);

        var result = ray.Transform(translation);

        var expectedResult = new Ray(Tuple.CreatePoint(4, 6, 8), Tuple.CreateVector(0, 1, 0));
        Assert.Equal(expectedResult, result, _rayComparer);
    }

    [Fact]
    public void ScaleRay()
    {
        var ray = new Ray(Tuple.CreatePoint(1, 2, 3), Tuple.CreateVector(0, 1, 0));
        var scale = _factory.Scale(2, 3, 4);

        var result = ray.Transform(scale);

        var expectedResult = new Ray(Tuple.CreatePoint(2, 6, 12), Tuple.CreateVector(0, 3, 0));
        Assert.Equal(expectedResult, result, _rayComparer);
    }
}