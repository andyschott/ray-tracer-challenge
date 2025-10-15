using RayTracerChallenge.Domain;
using RayTracerChallenge.Extensions;
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
        Assert.Same(s, i.Shape);
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
        Assert.Same(shape, result.Shape);

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
    
    [Fact]
    public void HitShouldOffsetPoint()
    {
        var r = new Ray(Tuple.CreatePoint(0, 0, -5),
            Tuple.CreateVector(0, 0, 1));
        var s = new Sphere(Matrix.Identity.Translate(0, 0, 1));
        var i = new Intersection(5, s);
        var comps = i.PrepareComputations(r);
        
        Assert.True(comps.OverPoint.Z < -Constants.Epsilon / 2);
        Assert.True(comps.Point.Z > comps.OverPoint.Z);
    }
}