using RayTracerChallenge.Domain;
using Tuple = RayTracerChallenge.Domain.Tuple;

namespace RayTracerChallenge.Tests.Domain;

public class PointLightTests
{
    [Fact]
    public void ConstructsSuccessfully()
    {
        var c = new Color(1, 1, 1);
        var position = Tuple.CreatePoint(0, 0, 0);
        
        var light = new PointLight(position, c);
        
        Assert.Same(c, light.Intensity);
        Assert.Same(position, light.Position);
    }
}