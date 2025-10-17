using RayTracerChallenge.Domain;
using RayTracerChallenge.Domain.Patterns;
using Tuple = RayTracerChallenge.Domain.Tuple;

namespace RayTracerChallenge.Tests.Domain.Patterns;

public class RingPatternTests
{
    private readonly Color _black = new Color(0, 0, 0);
    private readonly Color _white = new Color(1, 1, 1);

    [Fact]
    public void RingShouldExtendInXAndZ()
    {
        var pattern = new RingPattern(_white, _black);

        var result = pattern.ColorAt(Tuple.CreatePoint(0, 0, 0));
        Assert.Equal(_white, result);
        
        result = pattern.ColorAt(Tuple.CreatePoint(1, 0, 0));
        Assert.Equal(_black, result);
        
        result = pattern.ColorAt(Tuple.CreatePoint(0, 0, 1));
        Assert.Equal(_black, result);
        
        result = pattern.ColorAt(Tuple.CreatePoint(0.708, 0, 0.708));
        Assert.Equal(_black, result);
    }
}