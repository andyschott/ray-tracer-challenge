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
            .With(tuple => tuple.W, 1)
            .Create();

        Assert.True(tuple.IsPoint);
        Assert.False(tuple.IsVector);
    }

    [Fact]
    public void TupleIsVector()
    {
        var tuple = _fixture.Build<Tuple>()
            .With(tuple => tuple.W, 0)
            .Create();

        Assert.False(tuple.IsPoint);
        Assert.True(tuple.IsVector);
    }

    [Fact]
    public void CreateTupleAsPoint()
    {
        var x = _fixture.Create<decimal>();
        var y = _fixture.Create<decimal>();
        var z = _fixture.Create<decimal>();

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
        var x = _fixture.Create<decimal>();
        var y = _fixture.Create<decimal>();
        var z = _fixture.Create<decimal>();

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

    [Fact]
    public void SubtractPointFromPoint()
    {
        var point1 = CreatePoint();
        var point2 = CreatePoint();

        var result = point1.Subtract(point2);

        var expectedTuple = new Tuple
        {
            X = point1.X - point2.X,
            Y = point1.Y - point2.Y,
            Z = point1.Z - point2.Z,
            W = Tuple.Vector
        };

        Assert.Equal(expectedTuple, result, _comparer);
    }

    [Fact]
    public void SubtractVectorFromPoint()
    {
        var point = CreatePoint();
        var vector = CreateVector();
        
        var result = point.Subtract(vector);

        var expectedTuple = new Tuple
        {
            X = point.X - vector.X,
            Y = point.Y - vector.Y,
            Z = point.Z - vector.Z,
            W = Tuple.Point
        };

        Assert.Equal(expectedTuple, result, _comparer);
    }

    [Fact]
    public void SubtractVectorFromVector()
    {
        var vector1 = CreateVector();
        var vector2 = CreateVector();
        
        var result = vector1.Subtract(vector2);

        var expectedTuple = new Tuple
        {
            X = vector1.X - vector2.X,
            Y = vector1.Y - vector2.Y,
            Z = vector1.Z - vector2.Z,
            W = Tuple.Vector
        };

        Assert.Equal(expectedTuple, result, _comparer);
    }

    [Fact]
    public void ErrorSubtractingPointFromVector()
    {
        var vector = CreateVector();
        var point = CreatePoint();

        Assert.Throws<ArgumentException>(() => vector.Subtract(point));
    }

    [Fact]
    public void SubtractVectorFromZeroVector()
    {
        var zero = Tuple.CreateVector(0, 0, 0);
        var vector = CreateVector();

        var result = zero.Subtract(vector);

        var expectedTuple = new Tuple
        {
            X = vector.X * -1,
            Y = vector.Y * -1,
            Z = vector.Z * -1,
            W = Tuple.Vector
        };

        Assert.Equal(expectedTuple, result, _comparer);
    }

    [Fact]
    public void NegateATuple()
    {
        var tuple = _fixture.Create<Tuple>();

        var result = tuple.Negate();

        var expectedTuple = new Tuple
        {
            X = tuple.X * -1,
            Y = tuple.Y * -1,
            Z = tuple.Z * -1,
            W = tuple.W * -1
        };

        Assert.Equal(expectedTuple, result, _comparer);
    }

    [Fact]
    public void MultplyTupleByScalar()
    {
        var tuple = _fixture.Create<Tuple>();
        var factor = _fixture.Create<decimal>();

        var result = tuple.Multiply(factor);

        var expectedTuple = new Tuple
        {
            X = tuple.X * factor,
            Y = tuple.Y * factor,
            Z = tuple.Z * factor,
            W = tuple.W * factor
        };

        Assert.Equal(expectedTuple, result, _comparer);
    }

    [Fact]
    public void DivideTupleByScalar()
    {
        var tuple = _fixture.Create<Tuple>();
        var factor = _fixture.Create<decimal>();

        var result = tuple.Divide(factor);

        var expectedTuple = new Tuple
        {
            X = tuple.X / factor,
            Y = tuple.Y / factor,
            Z = tuple.Z / factor,
            W = tuple.W / factor
        };

        Assert.Equal(expectedTuple, result, _comparer);
    }

    [Fact]
    public void ErrorDividingByZero()
    {
        var tuple = _fixture.Create<Tuple>();

        Assert.Throws<DivideByZeroException>(() => tuple.Divide(0));
    }

    [Theory]
    [InlineData(1, 0, 0, 1)]
    [InlineData(0, 1, 0, 1)]
    [InlineData(0, 0, 1, 1)]
    [InlineData(1, 2, 3, 3.7416573868)]
    [InlineData(-1, -2, -3, 3.7416573868)]
    public void ComputeMagnitudeOfVector(decimal x, decimal y, decimal z, decimal expectedMagnitude)
    {
        var tuple = Tuple.CreateVector(x, y, z);

        var magnitude = tuple.Magnitude();

        Assert.True(Math.Abs(expectedMagnitude - magnitude) < 0.0001M);
    }

    [Theory]
    [InlineData(4, 0, 0, 4)]
    [InlineData(1, 2, 3, 3.7416573868F)]
    public void NormalizeVector(decimal x, decimal y, decimal z, decimal expectedMagnitude)
    {
        var vector = Tuple.CreateVector(x, y, z);

        var normalized = vector.Normalize();

        var expectedTuple = new Tuple
        {
            X = vector.X / expectedMagnitude,
            Y = vector.Y / expectedMagnitude,
            Z = vector.Z / expectedMagnitude,
            W = Tuple.Vector
        };
        Assert.Equal(expectedTuple, normalized, _comparer);
    }

    [Fact]
    public void MagntiudeOfNormalizedVector()
    {
        var vector = CreateVector();

        var normalized = vector.Normalize();
        var magnitude = normalized.Magnitude();

        Assert.True(Math.Abs(1 - magnitude) < 0.0001M);
    }

    [Fact]
    public void DotProduct()
    {
        var vector1 = CreateVector();
        var vector2 = CreateVector();

        var dotProduct = vector1.DotProduct(vector2);

        var expectedDotProduct = vector1.X * vector2.X +
            vector1.Y * vector2.Y +
            vector1.Z * vector2.Z +
            vector1.W * vector2.W;

        Assert.Equal(expectedDotProduct, dotProduct);
    }

    [Fact]
    public void CrossProduct()
    {
        var vector1 = CreateVector();
        var vector2 = CreateVector();

        var crossProduct = vector1.CrossProduct(vector2);

        var expectedTuple = Tuple.CreateVector(
            vector1.Y * vector2.Z - vector1.Z * vector2.Y,
            vector1.Z * vector2.X - vector1.X * vector2.Z,
            vector1.X * vector2.Y - vector1.Y * vector2.X);

        Assert.Equal(expectedTuple, crossProduct, _comparer);
    }

    [Fact]
    public void InvalidCrossProduct_ThisIsPoint()
    {
        var tuple1 = CreatePoint();
        var tuple2 = CreateVector();

        Assert.Throws<Exception>(() => tuple1.CrossProduct(tuple2));
    }

    [Fact]
    public void InvalidCrossProduct_OtherIsPoint()
    {
        var tuple1 = CreateVector();
        var tuple2 = CreatePoint();

        Assert.Throws<ArgumentException>(() => tuple1.CrossProduct(tuple2));
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
