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

    [Fact]
    public void PrecomputeReflectionVector()
    {
        var shape = new Plane();
        var r = new Ray(0, 1, -1, 0, -1 * Math.Sqrt(2)/2, Math.Sqrt(2)/2);
        var i = new Intersection(Math.Sqrt(2), shape);
        
        var result = i.PrepareComputations(r);
        var expectedResult = Tuple.CreateVector(0, Math.Sqrt(2)/2, Math.Sqrt(2)/2);
        
        Assert.Equal(expectedResult, result.ReflectionVector);
    }

    [Theory]
    [InlineData(0, 1, 1.5)]
    [InlineData(1, 1.5, 2)]
    [InlineData(2, 2, 2.5)]
    [InlineData(3, 2.5, 2.5)]
    [InlineData(4, 2.5, 1.5)]
    [InlineData(5, 1.5, 1.0)]
    public void VerifyRefraction(int index, double n1, double n2)
    {
        var a = Sphere.Glass(Matrix.Identity.Scale(2, 2, 2),
            refractiveIndex: 1.5);
        var b = Sphere.Glass(Matrix.Identity.Translate(0, 0, -0.25),
            refractiveIndex: 2);
        var c = Sphere.Glass(Matrix.Identity.Translate(0, 0, 0.25),
            refractiveIndex: 2.5);

        var r = new Ray(0, 0, -4, 0, 0, 1);
        var xs = new Intersections
        {
            new Intersection(2, a),
            new Intersection(2.75, b),
            new Intersection(3.25, c),
            new Intersection(4.75, b),
            new Intersection(5.25, c),
            new Intersection(6, a),
        };
        
        var result = xs[index].PrepareComputations(r, xs);
        
        Assert.Equal(n1, result.N1);
        Assert.Equal(n2, result.N2);
    }

    [Fact]
    public void UnderPointIsOffsetBelowSurface()
    {
        var r = new Ray(0, 0, -5, 0, 0, 1);
        var shape = Sphere.Glass(Matrix.Identity.Translate(0, 0, 1));
        var i = new Intersection(5, shape);
        
        var result = i.PrepareComputations(r);
        
        Assert.True(result.UnderPoint.Z > Constants.Epsilon / 2);
        Assert.True(result.Point.Z < result.UnderPoint.Z);
    }

    [Fact]
    public void SchlickApproximationUnderTotalInternalReflection()
    {
        var shape = Sphere.Glass();
        var r = new Ray(0, 0, Math.Sqrt(2) / 2, 0, 1, 0);
        var xs = new Intersections
        {
            new Intersection(-1 * Math.Sqrt(2) / 2, shape),
            new Intersection(Math.Sqrt(2) / 2, shape),
        };
        var comps = xs[1].PrepareComputations(r, xs);
        
        var result = comps.Schlick();
        Assert.Equal(1, result);
    }

    [Fact]
    public void SchlickApproximation_PerpendicularViewingAngle()
    {
        var shape = Sphere.Glass();
        var r = new Ray(0, 0, 0, 0, 1, 0);
        var xs = new Intersections
        {
            new Intersection(-1, shape),
            new Intersection(1, shape)
        };
        var comps = xs[1].PrepareComputations(r, xs);
        
        var result = comps.Schlick();
        Assert.Equal(0.04, result, 5);
    }

    [Fact]
    public void SchlickApproximation_SmallAngleAndN2GreaterThanN1()
    {
        var shape = Sphere.Glass();
        var r = new Ray(0, 0.99, -2, 0, 0, 1);
        var i = new Intersection(1.8589, shape);
        var comps = i.PrepareComputations(r);
        
        var result = comps.Schlick();
        Assert.Equal(0.48873, result, 5);
    }
}