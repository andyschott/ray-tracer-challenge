using RayTracerChallenge.Domain;
using RayTracerChallenge.Extensions;
using Tuple = RayTracerChallenge.Domain.Tuple;

namespace RayTracerChallenge.Tests.Domain;

public class StripePatternTests
{
    private readonly Color _black = new Color(0, 0, 0);
    private readonly Color _white = new Color(1, 1, 1);

    [Fact]
    public void StripePattern()
    {
        var pattern = new StripePattern(_white, _black);
        
        Assert.Same(_white, pattern.A);
        Assert.Same(_black, pattern.B);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void StripePatternConstantY(double y)
    {
        var pattern = new StripePattern(_white, _black);

        var result = pattern.ColorAt(Tuple.CreatePoint(0, y, 0));
        Assert.Same(_white, result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void StripePatternConstantZ(double z)
    {
        var pattern = new StripePattern(_white, _black);

        var result = pattern.ColorAt(Tuple.CreatePoint(0, 0, z));
        Assert.Same(_white, result);
    }

    [Theory]
    [InlineData(0, true)]
    [InlineData(0.9, true)]
    [InlineData(1, false)]
    [InlineData(-0.1, false)]
    [InlineData(-1, false)]
    [InlineData(-1.1, true)]
    public void StripePatternAlternatesX(double x, bool whichColor)
    {
        var pattern = new StripePattern(_white, _black);

        var result = pattern.ColorAt(Tuple.CreatePoint(x, 0, 0));
        var expectedResult = whichColor ? _white : _black;
        Assert.Same(expectedResult, result);
    }

    [Fact]
    public void StripesWithObjectTransformation()
    {
        var s = new Sphere(Matrix.Identity.Scale(2, 2, 2));
        var pattern = new StripePattern(_white, _black);
        
        var result = pattern.ColorAtForObject(s,
            Tuple.CreatePoint(1.5, 0, 0));
        Assert.Same(_white, result);
    }

    [Fact]
    public void StripesWithPatternTransformation()
    {
        var s = new Sphere();
        var pattern = new StripePattern(_white, _black,
            Matrix.Identity.Scale(2, 2, 2));
        
        var result = pattern.ColorAtForObject(s,
            Tuple.CreatePoint(1.5, 0, 0));
        Assert.Same(_white, result);
    }

    [Fact]
    public void StripesWithObjectAndPatternTransformation()
    {
        var s = new Sphere(Matrix.Identity.Scale(2, 2, 2));
        var pattern = new StripePattern(_white, _black,
            Matrix.Identity.Translate(0.5, 0, 0));
        
        var result = pattern.ColorAtForObject(s,
            Tuple.CreatePoint(2.5, 0, 0));
        Assert.Same(_white, result);
    }
}