using RayTracerChallenge.Domain;

namespace RayTracerChallenge.Tests.Domain;

public class CanvasTests
{
    [Fact]
    public void ConstructsSuccessfully()
    {
        var c = new Canvas(10, 20);
        
        Assert.Equal(10, c.Width);
        Assert.Equal(20, c.Height);

        var initialColor = new Color(0, 0, 0);
        Assert.All(c, pixel => Assert.Equal(initialColor, pixel));
    }

    [Fact]
    public void WritingPixelsToACanvas()
    {
        var c = new Canvas(10, 20);
        var red = new Color(1, 0, 0);

        c[2, 3] = red;
        
        Assert.Equal(red, c[2, 3]);
    }
}