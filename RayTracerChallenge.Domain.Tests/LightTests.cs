using System;

namespace RayTracerChallenge.Domain.Tests;

public class LightTests
{
    private readonly IFixture _fixture = new Fixture();

    private readonly LightComparer _lightComparer = new LightComparer();
    private readonly TupleComparer _tupleComparer = new TupleComparer();
    private readonly ColorComparer _colorComparer = new ColorComparer();

    [Fact]
    public void LightRequiresPoint()
    {
        var vector = _fixture.Build<Tuple>()
            .With(t => t.W, Tuple.Vector)
            .Create();
        var intensity = _fixture.Create<Color>();

        Assert.Throws<ArgumentException>(() => new Light(vector, intensity));
    }

    [Fact]
    public void LightHasPositionAndIntensity()
    {
        var position = Tuple.CreatePoint(0, 0, 0);
        var intensity = new Color
        {
            Red = 255,
            Green = 255,
            Blue = 255
        };

        var light = new Light(position, intensity);
        
        Assert.Equal(position, light.Position, _tupleComparer);
        Assert.Equal(intensity, light.Intensity, _colorComparer);
    }
}
