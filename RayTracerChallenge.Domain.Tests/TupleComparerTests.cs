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
}
