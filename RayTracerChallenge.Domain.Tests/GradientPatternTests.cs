namespace RayTracerChallenge.Domain.Tests;
public class GradientPatternTests
{
    private readonly ColorComparer _colorComparer = new ColorComparer();

    [Theory]
    [InlineData(0, 0, 0, 1, 1, 1)]
    [InlineData(0.25, 0, 0, 0.75, 0.75, 0.75)]
    [InlineData(0.5, 0, 0, 0.5, 0.5, 0.5)]
    [InlineData(0.75, 0, 0, 0.25, 0.25, 0.25)]
    public void ConstructGradientPattern(decimal x, decimal y, decimal z,
        decimal red, decimal green, decimal blue)
    {
        var pattern = new GradientPattern(Color.White, Color.Black);

        var result = pattern.ColorAt(Tuple.CreatePoint(x, y, z));

        var expectedResult = new Color
        {
            Red = red,
            Green = green,
            Blue = blue
        };
        Assert.Equal(expectedResult, result, _colorComparer);
    }
}
