using RayTracerChallenge.Domain;
using RayTracerChallenge.Domain.Patterns;
using Tuple = RayTracerChallenge.Domain.Tuple;

namespace RayTracerChallenge.Tests.Domain.Patterns;

public class GradientPatternTests
{
    private readonly Color _black = new Color(0, 0, 0);
    private readonly Color _white = new Color(1, 1, 1);

    [Fact]
    public void GradientLinearlyInterpolatesBetweenColors()
    {
        var pattern = new GradientPattern(_white, _black);

        var result = pattern.ColorAt(Tuple.CreatePoint(0, 0, 0));
        Assert.Equal(_white, result);
        
        result = pattern.ColorAt(Tuple.CreatePoint(0.25, 0, 0));
        Assert.Equal(new Color(0.75, 0.75, 0.75),
            result);
        
        result = pattern.ColorAt(Tuple.CreatePoint(0.5, 0, 0));
        Assert.Equal(new Color(0.5, 0.5, 0.5),
            result);
        
        result = pattern.ColorAt(Tuple.CreatePoint(0.75, 0, 0));
        Assert.Equal(new Color(0.25, 0.25, 0.25),
            result);
    }
}