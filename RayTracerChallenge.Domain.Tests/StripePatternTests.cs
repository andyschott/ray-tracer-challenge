using System;
using RayTracerChallenge.Domain.Tests.Extensions;

namespace RayTracerChallenge.Domain.Tests;

public class StripePatternTests
{
    private readonly TransformationFactory _factory = new TransformationFactory();

    private readonly IFixture _fixture = new Fixture();

    private readonly ColorComparer _colorComparer = new ColorComparer();

    [Fact]
    public void ConstructStripePattern()
    {
        var pattern = new StripePattern(Color.White, Color.Black);

        Assert.Equal(Color.White, pattern.First);
        Assert.Equal(Color.Black, pattern.Second);
    }

    [Fact]
    public void ColorAtRequiresPoint()
    {
        var pattern = new StripePattern(Color.White, Color.Black);
        var vector = _fixture.CreateVector();

        Assert.Throws<ArgumentException>(() => pattern.ColorAt(vector));
    }

    [Fact]
    public void ColorIsConstantInY()
    {
        var pattern = new StripePattern(Color.White, Color.Black);
        var point = Tuple.CreatePoint(0, _fixture.Create<decimal>(), 0);

        var result = pattern.ColorAt(point);

        Assert.Equal(pattern.First, result);
    }

    [Fact]
    public void ColorIsConstantInZ()
    {
        var pattern = new StripePattern(Color.White, Color.Black);
        var point = Tuple.CreatePoint(0, 0, _fixture.Create<decimal>());

        var result = pattern.ColorAt(point);

        Assert.Equal(pattern.First, result);
    }

    [Theory]
    [InlineData(0, 0, 0, true)]
    [InlineData(0, 0, 0, true)]
    [InlineData(1, 0, 0, false)]
    [InlineData(-0.1, 0, 0, false)]
    [InlineData(-1, 0, 0, false)]
    [InlineData(-1.1, 0, 0, true)]
    public void ColorAlternatesInX(decimal x, decimal y, decimal z, bool expectFirstColor)
    {
        var pattern = new StripePattern(Color.White, Color.Black);
        var point = Tuple.CreatePoint(x, y, z);

        var result = pattern.ColorAt(point);

        Assert.Equal(expectFirstColor ? pattern.First : pattern.Second, result);
    }

    [Fact]
    public void ObjectTransformation()
    {
        var shape = new TestShape
        {
            Transform = _factory.Scale(2, 2, 2)
        };
        var pattern = new StripePattern(Color.White, Color.Black);

        var result = pattern.ColorAt(shape, Tuple.CreatePoint(1.5M, 0, 0));

        Assert.Equal(Color.White, result, _colorComparer);
    }

    [Fact]
    public void PatternTransformation()
    {
        var shape = new TestShape();
        var pattern = new StripePattern(Color.White, Color.Black,
            _factory.Scale(2, 2, 2));
        
        var result = pattern.ColorAt(shape, Tuple.CreatePoint(1.5M, 0, 0));

        Assert.Equal(Color.White, result, _colorComparer);
    }

    [Fact]
    public void ObjectAndShapeTransformation()
    {
        var shape = new TestShape
        {
            Transform = _factory.Scale(2, 2, 2)
        };
        var pattern = new StripePattern(Color.White, Color.Black,
            _factory.Translation(0.5M, 0, 0));

        var result = pattern.ColorAt(shape, Tuple.CreatePoint(2.5M, 0, 0));

        Assert.Equal(Color.White, result, _colorComparer);
    }
}
