namespace RayTracerChallenge.Domain.Tests;

public class TupleTests
{
    private readonly IFixture _fixture = new Fixture();

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

        Assert.Equal(x, tuple.X);
        Assert.Equal(y, tuple.Y);
        Assert.Equal(z, tuple.Z);
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

        Assert.Equal(x, tuple.X);
        Assert.Equal(y, tuple.Y);
        Assert.Equal(z, tuple.Z);
        Assert.False(tuple.IsPoint);
        Assert.True(tuple.IsVector);
    }
}