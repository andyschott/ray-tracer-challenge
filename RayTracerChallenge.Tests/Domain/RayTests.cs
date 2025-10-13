using RayTracerChallenge.Domain;
using RayTracerChallenge.Extensions;
using Tuple = RayTracerChallenge.Domain.Tuple;

namespace RayTracerChallenge.Tests.Domain;

public class RayTests
{
    [Fact]
    public void CreateRay()
    {
        var origin = Tuple.CreatePoint(1, 2, 3);
        var direction = Tuple.CreateVector(4, 5, 6);
        
        var ray = new Ray(origin, direction);
        
        Assert.Same(origin, ray.Origin);
        Assert.Same(direction, ray.Direction);
    }

    [Fact]
    public void ComputePointFromDistance()
    {
        var ray = new Ray(Tuple.CreatePoint(2, 3, 4),
            Tuple.CreateVector(1, 0, 0));

        var result = ray.CalculatePosition(0);
        var expectedResult = Tuple.CreatePoint(2, 3, 4);
        Assert.Equal(expectedResult, result);

        result = ray.CalculatePosition(1);
        expectedResult = Tuple.CreatePoint(3, 3, 4);
        Assert.Equal(expectedResult, result);
        
        result = ray.CalculatePosition(-1);
        expectedResult = Tuple.CreatePoint(1, 3, 4);
        Assert.Equal(expectedResult, result);
        
        result = ray.CalculatePosition(2.5M);
        expectedResult = Tuple.CreatePoint(4.5M, 3, 4);
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void TranslatingRay()
    {
        var r = new Ray(Tuple.CreatePoint(1, 2, 3),
            Tuple.CreateVector(0, 1, 0));
        var m = Matrix.Identity
            .Translate(3, 4, 5);
        var r2 = r.Transform(m);
        
        var expectedResult = new Ray(Tuple.CreatePoint(4, 6, 8),
            Tuple.CreateVector(0, 1, 0));
        Assert.NotSame(r, r2);
        Assert.Equal(expectedResult, r2);
    }

    [Fact]
    public void ScalingRay()
    {
        var r = new Ray(Tuple.CreatePoint(1, 2, 3),
            Tuple.CreateVector(0, 1, 0));
        var m = Matrix.Identity
            .Scale(2, 3, 4);
        var r2 = r.Transform(m);
        
        var expectedResult = new Ray(Tuple.CreatePoint(2, 6, 12),
            Tuple.CreateVector(0, 3, 0));
        Assert.NotSame(r, r2);
        Assert.Equal(expectedResult, r2);
    }
}