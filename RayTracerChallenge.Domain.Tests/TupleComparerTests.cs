using System.Collections.Generic;

namespace RayTracerChallenge.Domain.Tests;

public class TupleComparerTests : AbstractComparerTests<Tuple>
{
    public TupleComparerTests() : base(new TupleComparer())
    {
    }

    [Fact]
    public void SameValuesAreEqual()
    {
        var x = _fixture.Create<decimal>();
        var y = _fixture.Create<decimal>();
        var z = _fixture.Create<decimal>();
        var w = _fixture.Create<decimal>();

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
}
