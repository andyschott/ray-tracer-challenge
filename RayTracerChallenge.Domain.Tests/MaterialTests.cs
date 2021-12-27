namespace RayTracerChallenge.Domain.Tests;

public class MaterialTests
{
    private readonly ColorComparer _colorComparer = new ColorComparer();

    [Fact]
    public void DefaultValues()
    {
        var material = new Material();

        var black = new Color
        {
            Red = 1,
            Green = 1,
            Blue = 1
        };

        Assert.Equal(black, material.Color, _colorComparer);
        Assert.Equal(0.1M, material.Ambient);
        Assert.Equal(0.9M, material.Diffuse);
        Assert.Equal(0.9M, material.Specular);
        Assert.Equal(200.0M, material.Shininess);
    }    
}
