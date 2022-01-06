using RayTracerChallenge.Domain.Patterns;
using RayTracerChallenge.Domain.Tests.Extensions;

namespace RayTracerChallenge.Domain.Tests.Patterns;

public class SolidPatternTests
{
    private readonly IFixture _fixture = new Fixture();

    private readonly ColorComparer _colorComparer = new ColorComparer();

    [Fact]
    public void AlwaysReturnsColor()
    {
        var color = _fixture.Create<Color>();
        var pattern = new SolidPattern(color);

        var point = _fixture.CreatePoint();

        var result = pattern.ColorAt(point);

        Assert.Equal(color, result, _colorComparer);
    }
}
