using System;

namespace RayTracerChallenge.Domain.Tests;

public class RayTests
{
    private TupleComparer _tupleComparer = new TupleComparer();

    [Fact]
    public void CreateRay()
    {
        var origin = Tuple.CreatePoint(1, 2, 3);
        var direction = Tuple.CreateVector(4, 5, 6);

        var ray = new Ray(origin, direction);

        Assert.Equal(origin, ray.Origin, _tupleComparer);
        Assert.Equal(direction, ray.Direction, _tupleComparer);
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
}