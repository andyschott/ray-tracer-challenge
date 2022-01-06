using RayTracerChallenge.Domain.Patterns;

namespace RayTracerChallenge.Domain.Tests.Patterns;

public class RingPatternTests
{
    private readonly ColorComparer _colorComparer = new ColorComparer();

    [Theory]
    [InlineData(0, 0, true)]
    [InlineData(1, 0, false)]
    [InlineData(0, 1, false)]
    public void RingExtendsInXAndZ(decimal x, decimal z, bool expectFirstColor)
    {
        var pattern = new RingPattern(Color.White, Color.Black);

        var result = pattern.ColorAt(Tuple.CreatePoint(x, 0, z));

        Assert.Equal(expectFirstColor ? pattern.First : pattern.Second, result, _colorComparer);
    }
}