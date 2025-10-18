using RayTracerChallenge.Domain;
using RayTracerChallenge.Domain.Shapes;
using Tuple = RayTracerChallenge.Domain.Tuple;

namespace RayTracerChallenge.Tests.Domain;

public class TriangleTests
{
    [Fact]
    public void ConstructingTriangle()
    {
        var p1 = Tuple.CreatePoint(0, 1, 0);
        var p2 = Tuple.CreatePoint(-1, 0, 0);
        var p3 = Tuple.CreatePoint(1, 0, 0);
        var t = new Triangle(p1, p2, p3);
        
        Assert.Equal(p1, t.Point1);
        Assert.Equal(p2, t.Point2);
        Assert.Equal(p3, t.Point3);

        var edge1 = Tuple.CreateVector(-1, -1, 0);
        Assert.Equal(edge1, t.Edge1);
        
        var edge2 = Tuple.CreateVector(1, -1, 0);
        Assert.Equal(edge2, t.Edge2);

        var normal = Tuple.CreateVector(0, 0, -1);
        Assert.Equal(normal, t.Normal);
    }

    [Theory]
    [InlineData(0, 0.5, 0)]
    [InlineData(-0.5, 0.75, 0)]
    [InlineData(0.5, 0.25, 0)]
    public void NormalOfTriangle(double x, double y, double z)
    {
        var t = new Triangle(0, 1, 0,
            -1, 0, 0,
            1, 0, 0);

        var result = t.NormalAt(Tuple.CreatePoint(x, y, z),
            new Intersection(1, t));
        
        Assert.Equal(t.Normal, result);
    }

    [Fact]
    public void IntersectingRayParallelToTriangle()
    {
        var t = new Triangle(0, 1, 0,
            -1, 0, 0,
            1, 0, 0);
        var r = new Ray(0, -1, -2, 0, 1, 0);

        var xs = t.Intersects(r);
        
        Assert.Empty(xs);
    }

    [Theory]
    [InlineData(1, 1, -2, 0, 0, 1)]
    [InlineData(-1, 1, -2, 0, 0, 1)]
    [InlineData(0, -1, -2, 0, 0, 1)]
    public void RayMissesEdge(double x, double y, double z,
        double vx, double vy, double vz)
    {
        var t = new Triangle(0, 1, 0,
            -1, 0, 0,
            1, 0, 0);
        var r = new Ray(x, y, z, vx, vy, vz);
        
        var xs = t.Intersects(r);
        
        Assert.Empty(xs);
    }

    [Fact]
    public void RayStrikesTriangle()
    {
        var t = new Triangle(0, 1, 0,
            -1, 0, 0,
            1, 0, 0);
        var r = new Ray(0, 0.5, -2, 0, 0, 1);
        
        var xs = t.Intersects(r);

        var i = Assert.Single(xs);
        Assert.Equal(2, i.T);
    }
}