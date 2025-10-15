using RayTracerChallenge.Domain;
using Tuple = RayTracerChallenge.Domain.Tuple;

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

    [Fact]
    public void PrecomputingStateOfIntersection()
    {
        var r = new Ray(Tuple.CreatePoint(0, 0, -5),
            Tuple.CreateVector(0, 0, 1));
        var shape = new Sphere();
        var i = new Intersection(4, shape);

        var result = i.PrepareComputations(r);
        
        Assert.Equal(i.T, result.T);
        Assert.Same(shape, result.Object);

        var expectedPoint = Tuple.CreatePoint(0, 0, -1);
        Assert.Equal(expectedPoint, result.Point);
        
        var expectedEyeVector = Tuple.CreateVector(0, 0, -1);
        Assert.Equal(expectedEyeVector, result.EyeVector);
        
        var expectedNormalVector = Tuple.CreateVector(0, 0, -1);
        Assert.Equal(expectedNormalVector, result.NormalVector);
        
        Assert.False(result.IsInside);
    }

    [Fact]
    public void HitWhenIntersectionIsInside()
    {
        var r = new Ray(Tuple.CreatePoint(0, 0, 0),
            Tuple.CreateVector(0, 0, 1));
        var shape = new Sphere();
        var i = new Intersection(1, shape);

        var result = i.PrepareComputations(r);
        
        var expectedPoint = Tuple.CreatePoint(0, 0, 1);
        Assert.Equal(expectedPoint, result.Point);
        
        var expectedEyeVector = Tuple.CreateVector(0, 0, -1);
        Assert.Equal(expectedEyeVector, result.EyeVector);
        
        var expectedNormalVector = Tuple.CreateVector(0, 0, -1);
        Assert.Equal(expectedNormalVector, result.NormalVector);
        
        Assert.True(result.IsInside);
    }
}