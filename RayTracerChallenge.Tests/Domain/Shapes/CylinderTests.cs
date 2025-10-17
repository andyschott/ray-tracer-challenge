using RayTracerChallenge.Domain;
using RayTracerChallenge.Domain.Shapes;
using Tuple = RayTracerChallenge.Domain.Tuple;

namespace RayTracerChallenge.Tests.Domain.Shapes;

public class CylinderTests
{
    [Theory]
    [InlineData(1, 0, 0, 0, 1, 0)]
    [InlineData(0, 0, 0, 0, 1, 0)]
    [InlineData(0, 0, -5, 1, 1, 1)]
    public void RayMissesCylinder(double x, double y, double z,
        double vx, double vy, double vz)
    {
        var c = new Cylinder();
        var ray = new Ray(x, y, z, vx, vy, vz);

        var result = c.Intersects(ray);

        Assert.Empty(result);
    }

    [Theory]
    [InlineData(1, 0, -5, 0, 0, 1, 5, 5)]
    [InlineData(0, 0, -5, 0, 0, 1, 4, 6)]
    [InlineData(0.5, 0, -5, 0.1, 1, 1, 6.80798, 7.08872)]
    public void RayHitsCylinder(double x, double y, double z,
        double vx, double vy, double vz,
        double t0, double t1)
    {
        var c = new Cylinder();
        var pt = Tuple.CreatePoint(x, y, z);
        var v = Tuple.CreateVector(vx, vy, vz).Normalize();
        var ray = new Ray(pt, v);

        var result = c.Intersects(ray);

        Assert.Equal(2, result.Count);
        Assert.Equal(t0, result[0].T, 4);
        Assert.Equal(t1, result[1].T, 4);
    }

    [Theory]
    [InlineData(1, 0, 0, 1, 0, 0)]
    [InlineData(0, 5, -1, 0, 0, -1)]
    [InlineData(0, -2, 1, 0, 0, 1)]
    [InlineData(-1, 1, 0, -1, 0, 0)]
    public void NormalVectorOfCylinder(double x, double y, double z,
        double nx, double ny, double nz)
    {
        var c = new Cylinder();
        var pt = Tuple.CreatePoint(x, y, z);

        var result = c.NormalAt(pt);
        var expectedResult = Tuple.CreateVector(nx, ny, nz);
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void DefaultCylinder()
    {
        var c = new Cylinder();
        
        Assert.Equal(double.NegativeInfinity, c.Minimum);
        Assert.Equal(double.PositiveInfinity, c.Maximum);
        Assert.False(c.Closed);
    }

    [Theory]
    [InlineData(0, 1.5, 0, 0.1, 1, 0, 0)]
    [InlineData(0, 3, -5, 0, 0, 1, 0)]
    [InlineData(0, 0, -5, 0, 0, 1, 0)]
    [InlineData(0, 2, -5, 0, 0, 1, 0)]
    [InlineData(0, 1, -5, 0, 0, 1, 0)]
    [InlineData(0, 1.5, -2, 0, 0, 1, 2)]
    public void IntersectingConstrainedCylinder(double x, double y, double z,
        double vx, double vy, double vz, int count)
    {
        var c = new Cylinder
        {
            Minimum = 1,
            Maximum = 2,
        };
        var pt = Tuple.CreatePoint(x, y, z);
        var direction = Tuple.CreateVector(vx, vy, vz).Normalize();
        var r = new Ray(pt, direction);
        
        var result = c.Intersects(r);
        
        Assert.Equal(count, result.Count);
    }

    [Theory]
    [InlineData(0, 3, 0, 0, -1, 0, 2)]
    [InlineData(0, 3, -2, 0, -1, 2, 2)]
    [InlineData(0, 4, -2, 0, -1, 1, 2)]
    [InlineData(0, 0, -2, 0, 1, 2, 2)]
    [InlineData(0, -1, -2, 0, 1, 1, 2)]
    public void IntersectingCapsOfClosedCylinder(double x, double y, double z,
        double vx, double vy, double vz, int count)
    {
        var c = new Cylinder
        {
            Minimum = 1,
            Maximum = 2,
            Closed = true
        };
        var pt = Tuple.CreatePoint(x, y, z);
        var direction = Tuple.CreateVector(vx, vy, vz).Normalize();
        var r = new Ray(pt, direction);
        
        var result = c.Intersects(r);
        
        Assert.Equal(count, result.Count);
    }

    [Theory]
    [InlineData(0, 1, 0, 0, -1, 0)]
    [InlineData(0.5, 1, 0, 0, -1, 0)]
    [InlineData(0, 1, 0.5, 0, -1, 0)]
    [InlineData(0, 2, 0, 0, 1, 0)]
    [InlineData(0.5, 2, 0, 0, 1, 0)]
    [InlineData(0, 2, 0.5, 0, 1, 0)]
    public void NormalVectorCylinderEndCaps(double x, double y, double z,
        double nx, double ny, double nz)
    {
        var c = new Cylinder
        {
            Minimum = 1,
            Maximum = 2,
            Closed = true
        };
        var pt = Tuple.CreatePoint(x, y, z);
        
        var result = c.NormalAt(pt);
        var expectedResult = Tuple.CreateVector(nx, ny, nz);
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void BoundsOfUnCappedCylinder()
    {
        var c = new Cylinder();
        var result = c.GetBounds();
        var expectedResult = new Bounds(-1, double.NegativeInfinity, -1,
            1, double.PositiveInfinity, 1);
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void BoundsOfCappedCylinder()
    {
        var c = new Cylinder
        {
            Minimum = 2,
            Maximum = 3,
        };
        var result = c.GetBounds();
        var expectedResult = new Bounds(-1, 2, -1,
            1, 3, 1);
        
        Assert.Equal(expectedResult, result);
    }
}