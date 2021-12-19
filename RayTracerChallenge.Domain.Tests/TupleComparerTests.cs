namespace RayTracerChallenge.Domain.Tests;

public class TupleComparerTests
{
    private readonly IFixture _fixture = new Fixture();
    private readonly TupleComparer _comparer = new TupleComparer();

    [Fact]
    public void BothTuplesNull()
    {
        var result = _comparer.Equals(null, null);

        Assert.True(result);
    }

    [Fact]
    public void FirstTupleNull()
    {
        var y = _fixture.Create<Tuple>();

        var result = _comparer.Equals(null, y);

        Assert.False(result);
    }

    [Fact]
    public void SecondTupleNull()
    {
        var x = _fixture.Create<Tuple>();

        var result = _comparer.Equals(x, null);
    }

    [Fact]
    public void TupleEqualsItself()
    {
        var tuple = _fixture.Create<Tuple>();
        
        var result = _comparer.Equals(tuple, tuple);

        Assert.True(result);
    }

    [Fact]
    public void SameValuesAreEqual()
    {
        var x = _fixture.Create<float>();
        var y = _fixture.Create<float>();
        var z = _fixture.Create<float>();
        var w = _fixture.Create<float>();

        var tuple1 = new Tuple
        {
            X = x,
            Y = y,
            Z = z,
            W = w
        };
        var tuple2 = new Tuple
        {
            X = x,
            Y = y,
            Z = z,
            W = w
        };

        var result = _comparer.Equals(tuple1, tuple2);

        Assert.True(result);
    }

    [Fact]
    public void DifferentValuesAreNotEqual()
    {
        var x = _fixture.Create<Tuple>();
        var y = _fixture.Create<Tuple>();

        var result = _comparer.Equals(x, y);

        Assert.False(result);
    }
}
