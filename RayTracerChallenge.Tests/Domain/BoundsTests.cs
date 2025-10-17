using RayTracerChallenge.Domain;
using Tuple = RayTracerChallenge.Domain.Tuple;

namespace RayTracerChallenge.Tests.Domain;

public class BoundsTests
{
    [Fact]
    public void ConstructedSuccessfully()
    {
        var min = Tuple.CreatePoint(1, 2, 3);
        var max = Tuple.CreatePoint(4, 5, 6);
        
        var bounds = new Bounds(min, max);
        
        Assert.Equal(min, bounds.Minimum);
        Assert.Equal(max, bounds.Maximum);
    }
}