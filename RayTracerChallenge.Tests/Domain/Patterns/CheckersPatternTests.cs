using RayTracerChallenge.Domain;
using RayTracerChallenge.Domain.Patterns;
using Tuple = RayTracerChallenge.Domain.Tuple;

namespace RayTracerChallenge.Tests.Domain.Patterns;

public class CheckersPatternTests
{
    private readonly Color _black = new Color(0, 0, 0);
    private readonly Color _white = new Color(1, 1, 1);

    [Fact]
    public void ShouldRepeatInX()
    {
        var pattern = new CheckersPattern(_white, _black);

        var result = pattern.ColorAt(Tuple.CreatePoint(0, 0, 0));
        Assert.Equal(_white, result);
        
        result = pattern.ColorAt(Tuple.CreatePoint(0.99, 0, 0));
        Assert.Equal(_white, result);
        
        result = pattern.ColorAt(Tuple.CreatePoint(1.01, 0, 0));
        Assert.Equal(_black, result);
    }

    [Fact]
    public void ShouldRepeatInY()
    {
        var pattern = new CheckersPattern(_white, _black);

        var result = pattern.ColorAt(Tuple.CreatePoint(0, 0, 0));
        Assert.Equal(_white, result);
        
        result = pattern.ColorAt(Tuple.CreatePoint(0, 0.99, 0));
        Assert.Equal(_white, result);
        
        result = pattern.ColorAt(Tuple.CreatePoint(0, 1.01, 0));
        Assert.Equal(_black, result);
    }

    [Fact]
    public void ShouldRepeatInZ()
    {
        var pattern = new CheckersPattern(_white, _black);

        var result = pattern.ColorAt(Tuple.CreatePoint(0, 0, 0));
        Assert.Equal(_white, result);
        
        result = pattern.ColorAt(Tuple.CreatePoint(0, 0, 0.99));
        Assert.Equal(_white, result);
        
        result = pattern.ColorAt(Tuple.CreatePoint(0, 0, 1.01));
        Assert.Equal(_black, result);
    }
}