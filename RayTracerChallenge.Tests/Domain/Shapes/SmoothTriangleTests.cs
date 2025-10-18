using RayTracerChallenge.Domain;
using RayTracerChallenge.Domain.Shapes;
using Tuple = RayTracerChallenge.Domain.Tuple;

namespace RayTracerChallenge.Tests.Domain.Shapes;

public class SmoothTriangleTests
{
    private readonly SmoothTriangle _triangle = new(0, 1, 0,
        -1, 0, 0,
        1, 0, 0,
        0, 1, 0,
        -1, 0, 0,
        1, 0, 0);

    [Fact]
    public void ConstructSuccessfully()
    {
        Assert.Equal(Tuple.CreatePoint(0, 1, 0), _triangle.Point1);
        Assert.Equal(Tuple.CreatePoint(-1, 0, 0), _triangle.Point2);
        Assert.Equal(Tuple.CreatePoint(1, 0, 0), _triangle.Point3);
        Assert.Equal(Tuple.CreateVector(0, 1, 0), _triangle.Normal1);
        Assert.Equal(Tuple.CreateVector(-1, 0, 0), _triangle.Normal2);
        Assert.Equal(Tuple.CreateVector(1, 0, 0), _triangle.Normal3);
    }

    [Fact]
    public void SmoothTriangleIntersectionsHaveUAndV()
    {
        var r = new Ray(-0.2, 0.3, -2, 0, 0, 1);
        var xs = _triangle.Intersects(r);
        
        var i= Assert.Single(xs);
        Assert.NotNull(i.U);
        Assert.Equal(0.45, i.U.Value, 4);
        Assert.NotNull(i.V);
        Assert.Equal(0.25, i.V.Value, 4);
    }

    [Fact]
    public void SmoothTriangleIntersectionsUseUAndV()
    {
        var i = new Intersection(1, _triangle, 0.45, 0.25);

        var result = _triangle.NormalAt(Tuple.CreatePoint(0, 0, 0), i);
        var expectedResult = Tuple.CreateVector(-0.5547, 0.83205, 0);
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void PrepareNormalOnSmoothTriangle()
    {
        var i = new Intersection(1, _triangle, 0.45, 0.25);
        var r = new Ray(-0.2, 0.3, -2, 0, 0, 1);

        var result = i.PrepareComputations(r);
        var expectedResult = Tuple.CreateVector(-0.5547, 0.83205, 0);
        
        Assert.Equal(expectedResult, result.NormalVector);
    }
}