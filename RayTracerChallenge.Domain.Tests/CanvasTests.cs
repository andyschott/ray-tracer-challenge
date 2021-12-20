using AutoFixture.AutoMoq;

namespace RayTracerChallenge.Domain.Tests;

public class CanvasTests
{
    private readonly IFixture _fixture = new Fixture()
        .Customize(new AutoMoqCustomization());

    private readonly ColorComparer _comparer = new ColorComparer();
    
    [Fact]
    public void CanvasInitializedToBlack()
    {
        var width = _fixture.Create<int>();
        var height = _fixture.Create<int>();

        var canvas = new Canvas(width, height);

        Assert.Equal(width, canvas.Width);
        Assert.Equal(height, canvas.Height);

        var black = new Color
        {
            Red = 0.0F,
            Green = 0.0F,
            Blue = 0.0F
        };
        Assert.All(canvas, pixel => Assert.Equal(black, pixel, _comparer));
    }

    [Fact]
    public void WritePixelToCanvas()
    {
        var canvas = _fixture.Create<Canvas>();

        var x = _fixture.Create<int>() % canvas.Width;
        var y = _fixture.Create<int>() % canvas.Height;

        var red = new Color
        {
            Red = 1.0F,
            Green = 0.0F,
            Blue = 0.0F
        };

        canvas.WritePixel(x, y, red);
        
        var pixel = canvas.GetPixel(x, y);

        Assert.Equal(red, pixel, _comparer);
    }
}
