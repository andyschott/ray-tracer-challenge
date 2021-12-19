using System;

namespace RayTracerChallenge.Domain.Tests;

public class TupleTests
{
    private readonly IFixture _fixture = new Fixture();
    private readonly TupleComparer _comparer = new TupleComparer();

    [Fact]
    public void TupleIsPoint()
    {
        var tuple = _fixture.Build<Tuple>()
            .With(tuple => tuple.W, 1.0)
            .Create();

        Assert.True(tuple.IsPoint);
        Assert.False(tuple.IsVector);
    }

    [Fact]
    public void TupleIsVector()
    {
        var tuple = _fixture.Build<Tuple>()
            .With(tuple => tuple.W, 0.0)
            .Create();

        Assert.False(tuple.IsPoint);
        Assert.True(tuple.IsVector);
    }

    [Fact]
    public void CreateTupleAsPoint()
    {
        var x = _fixture.Create<float>();
        var y = _fixture.Create<float>();
        var z = _fixture.Create<float>();

        var tuple = Tuple.CreatePoint(x, y, z);

        var expectedTuple = new Tuple
        {
            X = x,
            Y = y,
            Z = z,
            W = Tuple.Point
        };

        Assert.Equal(expectedTuple, tuple, _comparer);
        Assert.True(tuple.IsPoint);
        Assert.False(tuple.IsVector);
    }

    [Fact]
    public void CreateTupleAsVector()
    {
        var x = _fixture.Create<float>();
        var y = _fixture.Create<float>();
        var z = _fixture.Create<float>();

        var tuple = Tuple.CreateVector(x, y, z);

        var expectedTuple = new Tuple
        {
            X = x,
            Y = y,
            Z = z,
            W = Tuple.Vector
        };

        Assert.Equal(expectedTuple, tuple, _comparer);
        Assert.False(tuple.IsPoint);
        Assert.True(tuple.IsVector);
    }

    [Fact]
    public void AddVectorToPoint()
    {
        var point = CreatePoint();
        var vector = CreateVector();

        var result = point.Add(vector);

        var expectedTuple = new Tuple
        {
            X = point.X + vector.X,
            Y = point.Y + vector.Y,
            Z = point.Z + vector.Z,
            W = Tuple.Point
        };

        Assert.Equal(expectedTuple, result, _comparer);
    }

    [Fact]
    public void ErrorAddingPointToPoint()
    {
        var point1 = CreatePoint();
        var point2 = CreatePoint();

        Assert.Throws<ArgumentException>(() => point1.Add(point2));
    }

    private Tuple CreatePoint()
    {
        return _fixture.Build<Tuple>()
            .With(tuple => tuple.W, Tuple.Point)
            .Create();
    }

    private Tuple CreateVector()
    {
        return _fixture.Build<Tuple>()
            .With(tuple => tuple.W, Tuple.Vector)
            .Create();
    }
}
