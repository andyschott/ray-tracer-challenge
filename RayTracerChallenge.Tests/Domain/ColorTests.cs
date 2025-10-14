using RayTracerChallenge.Domain;

namespace RayTracerChallenge.Tests.Domain;

public class ColorTests
{
    [Fact]
    public void Color_ConstructsSuccessfully()
    {
        var c = new Color(-0.5, 0.4, 1.7);
        
        Assert.Equal(-0.5, c.Red);
        Assert.Equal(0.4, c.Green);
        Assert.Equal(1.7, c.Blue);
    }

    [Fact]
    public void AddingColors()
    {
        var c1 = new Color(0.9, 0.6, 0.75);
        var c2 = new Color(0.7, 0.1, 0.25);

        var sum = c1 + c2;
        var expectedResult = new Color(1.6, 0.7, 1.0);
        
        Assert.Equal(sum, expectedResult);
    }

    [Fact]
    public void SubtractingColors()
    {
        var c1 = new Color(0.9, 0.6, 0.75);
        var c2 = new Color(0.7, 0.1, 0.25);

        var sum = c1 - c2;
        var expectedResult = new Color(0.2, 0.5, 0.5);
        
        Assert.Equal(sum, expectedResult);
    }

    [Fact]
    public void MultiplyColorByScalar()
    {
        var c = new Color(0.2, 0.3, 0.4);

        var result = c * 2;
        var expectedResult = new Color(0.4, 0.6, 0.8);
        
        Assert.Equal(result, expectedResult);
    }

    [Fact]
    public void MultiplyingColors()
    {
        var c1 = new Color(1, 0.2, 0.4);
        var c2 = new Color(0.9, 1, 0.1);

        var product = c1 * c2;
        var expectedResult = new Color(0.9, 0.2, 0.04);
        
        Assert.Equal(product, expectedResult);
    }
}