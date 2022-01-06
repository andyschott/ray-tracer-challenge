using RayTracerChallenge.Domain.Patterns;

namespace RayTracerChallenge.Domain.Tests.Patterns;

public class CheckerPatternTests
{
    private ColorComparer _colorComparer = new ColorComparer();

    [Theory]
    [InlineData(0, 0, 0, true)]
    [InlineData(0.99, 0, 0, true)]
    [InlineData(1.01, 0, 0, false)]
    [InlineData(0, 0.99, 0, true)]
    [InlineData(0, 1.01, 0, false)]
    [InlineData(0, 0, 0.99, true)]
    [InlineData(0, 0, 1.01, false)]
    public void ShouldRepeatInAllDimensions(decimal x, decimal y, decimal z, bool expectFirstColor)
    {
        var pattern = new CheckerPattern(Color.White, Color.Black);

        var result = pattern.ColorAt(Tuple.CreatePoint(x, y, z));

        Assert.Equal(expectFirstColor ? pattern.First : pattern.Second, result, _colorComparer);
    }    
}
