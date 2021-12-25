using System.Collections.Generic;

namespace RayTracerChallenge.Domain.Tests;

public class ColorComparerTests : AbstractComparerTests<Color>
{
    public ColorComparerTests() : base(new ColorComparer())
    {
    }

    [Fact]
    public void SameValuesAreEqual()
    {
        var r = _fixture.Create<decimal>();
        var g = _fixture.Create<decimal>();
        var b = _fixture.Create<decimal>();

        var color1 = new Color
        {
            Red = r,
            Green = g,
            Blue = b
        };
        var color2 = new Color
        {
            Red = r,
            Green = g,
            Blue = b
        };

        var result = _comparer.Equals(color1, color2);

        Assert.True(result);
    }
}