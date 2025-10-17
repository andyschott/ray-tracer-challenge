using RayTracerChallenge.Domain;
using RayTracerChallenge.Domain.Shapes;
using Tuple = RayTracerChallenge.Domain.Tuple;

namespace RayTracerChallenge.Tests.Domain.Shapes;

public class PlaneTests
{
    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(10, 0, -10)]
    [InlineData(-5, 0, 150)]
    public void NormalOfPlaneIsConstant(double x,
        double y,
        double z)
    {
        var p = new Plane();

        var expectedResult = Tuple.CreateVector(0, 1, 0);
        
        var normal = p.NormalAt(Tuple.CreatePoint(x, y, z));
        Assert.Equal(expectedResult, normal);
    }

    [Fact]
    public void IntersectWithRayParallelToPlan()
    {
        var p = new Plane();
        var r = new Ray(Tuple.CreatePoint(0, 10, 0),
            Tuple.CreateVector(0, 0, 1));

        var xs = p.Intersects(r);

        Assert.Empty(xs);
    }

    [Fact]
    public void IntersectWithCoplanarRay()
    {
        var p = new Plane();
        var r = new Ray(Tuple.CreatePoint(0, 0, 0),
            Tuple.CreateVector(0, 0, 1));
        
        var xs = p.Intersects(r);
        
        Assert.Empty(xs);
    }

    [Fact]
    public void RayIntersectingPlaneFromAbove()
    {
        var p = new Plane();
        var r = new Ray(Tuple.CreatePoint(0, 1, 0),
            Tuple.CreateVector(0, -1, 0));
        
        var xs = p.Intersects(r);

        var intersection = Assert.Single(xs);
        Assert.Equal(1, intersection.T);
        Assert.Same(p, intersection.Shape);
    }

    [Fact]
    public void RayIntersectingPlaneFromBelow()
    {
        var p = new Plane();
        var r = new Ray(Tuple.CreatePoint(0, -1, 0),
            Tuple.CreateVector(0, 1, 0));
        
        var xs = p.Intersects(r);

        var intersection = Assert.Single(xs);
        Assert.Equal(1, intersection.T);
        Assert.Same(p, intersection.Shape);
    }

    [Fact]
    public void BoundsOfAPlane()
    {
        var p = new Plane();
        
        var result = p.GetBounds();
        var expectedResult = new Bounds(double.NegativeInfinity, 0, double.NegativeInfinity,
            double.PositiveInfinity, 0, double.PositiveInfinity);
        
        Assert.Equal(expectedResult, result);
    }
}