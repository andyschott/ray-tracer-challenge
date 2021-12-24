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
        var r = _fixture.Create<float>();
        var g = _fixture.Create<float>();
        var b = _fixture.Create<float>();

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