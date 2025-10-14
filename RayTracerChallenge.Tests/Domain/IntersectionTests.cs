using RayTracerChallenge.Domain;

namespace RayTracerChallenge.Tests.Domain;

public class IntersectionTests
{
    [Fact]
    public void IntersectionConstructedSuccessfully()
    {
        var s = new Sphere();
        var i = new Intersection(3.5, s);
        
        Assert.Equal(3.5, i.T);
        Assert.Same(s, i.Sphere);
    }
}