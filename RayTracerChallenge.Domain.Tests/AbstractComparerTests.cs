using System.Collections.Generic;

namespace RayTracerChallenge.Domain.Tests;

public abstract class AbstractComparerTests<T> where T : class
{
    protected readonly IFixture _fixture = new Fixture();
    protected readonly IEqualityComparer<T> _comparer;

    protected AbstractComparerTests(IEqualityComparer<T> comparer)
    {
        _comparer = comparer;
    }

    [Fact]
    public void BothValuesNull()
    {
        var result = _comparer.Equals(null, null);

        Assert.True(result);
    }

    [Fact]
    public void FirstValueNull()
    {
        var y = _fixture.Create<T>();

        var result = _comparer.Equals(null, y);

        Assert.False(result);
    }

    [Fact]
    public void SecondValueNull()
    {
        var x = _fixture.Create<T>();

        var result = _comparer.Equals(x, null);
    }

    [Fact]
    public void ValueEqualsItself()
    {
        var tuple = _fixture.Create<T>();
        
        var result = _comparer.Equals(tuple, tuple);

        Assert.True(result);
    }

    [Fact]
    public void DifferentValuesAreNotEqual()
    {
        var x = _fixture.Create<T>();
        var y = _fixture.Create<T>();

        var result = _comparer.Equals(x, y);

        Assert.False(result);
    }
}