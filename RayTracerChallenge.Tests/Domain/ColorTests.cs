using RayTracerChallenge.Domain;

namespace RayTracerChallenge.Tests.Domain;

public class ColorTests
{
    [Fact]
    public void Color_ConstructsSuccessfully()
    {
        var c = new Color(-0.5M, 0.4M, 1.7M);
        
        Assert.Equal(-0.5M, c.Red);
        Assert.Equal(0.4M, c.Green);
        Assert.Equal(1.7M, c.Blue);
    }

    [Fact]
    public void AddingColors()
    {
        var c1 = new Color(0.9M, 0.6M, 0.75M);
        var c2 = new Color(0.7M, 0.1M, 0.25M);

        var sum = c1 + c2;
        var expectedResult = new Color(1.6M, 0.7M, 1.0M);
        
        Assert.Equal(sum, expectedResult);
    }

    [Fact]
    public void SubtractingColors()
    {
        var c1 = new Color(0.9M, 0.6M, 0.75M);
        var c2 = new Color(0.7M, 0.1M, 0.25M);

        var sum = c1 - c2;
        var expectedResult = new Color(0.2M, 0.5M, 0.5M);
        
        Assert.Equal(sum, expectedResult);
    }

    [Fact]
    public void MultiplyColorByScalar()
    {
        var c = new Color(0.2M, 0.3M, 0.4M);

        var result = c * 2;
        var expectedResult = new Color(0.4M, 0.6M, 0.8M);
        
        Assert.Equal(result, expectedResult);
    }

    [Fact]
    public void MultiplyingColors()
    {
        var c1 = new Color(1, 0.2M, 0.4M);
        var c2 = new Color(0.9M, 1, 0.1M);

        var product = c1 * c2;
        var expectedResult = new Color(0.9M, 0.2M, 0.04M);
        
        Assert.Equal(product, expectedResult);
    }
}