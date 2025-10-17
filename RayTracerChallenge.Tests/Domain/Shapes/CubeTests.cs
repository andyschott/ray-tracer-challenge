using RayTracerChallenge.Domain;
using RayTracerChallenge.Domain.Shapes;
using Tuple = RayTracerChallenge.Domain.Tuple;

namespace RayTracerChallenge.Tests.Domain.Shapes;

public class CubeTests
{
    [Theory]
    [InlineData(5, 0.5, 0, -1, 0, 0, 4, 6)]
    [InlineData(-5, 0.5, 0, 1, 0, 0, 4, 6)]
    [InlineData(0.5, 5, 0, 0, -1, 0, 4, 6)]
    [InlineData(0.5, -5, 0, 0, 1, 0, 4, 6)]
    [InlineData(0.5, 0, 5, 0, 0, -1, 4, 6)]
    [InlineData(0.5, 0, -5, 0, 0, 1, 4, 6)]
    [InlineData(0, 0.5, 0, 0, 0, 1, -1, 1)]
    public void RayIntersectsCube(double x, double y, double z,
        double vx, double vy, double vz,
        double t1, double t2)
    {
        var c = new Cube();
        var r = new Ray(x, y, z, vx, vy, vz);
        
        var result = c.Intersects(r);
        
        Assert.Equal(2, result.Count);
        Assert.Equal(t1, result[0].T);
        Assert.Equal(t2, result[1].T);
    }

    [Theory]
    [InlineData(-2, 0, 0, 0.2673, 0.5345, 0.8018)]
    [InlineData(0, -2, 0, 0.8018, 0.2673, 0.5345)]
    [InlineData(0, 0, -2, 0.5345, 0.8018, 0.2673)]
    [InlineData(2, 0, 2, 0, 0, -1)]
    [InlineData(0, 2, 2, 0, -1, 0)]
    [InlineData(2, 2, 0, -1, 0, 0)]
    public void RayMissesCube(double x, double y, double z,
        double vx, double vy, double vz)
    {
        var c = new Cube();
        var r = new Ray(x, y, z, vx, vy, vz);
        
        var result = c.Intersects(r);
        
        Assert.Empty(result);
    }

    [Theory]
    [InlineData(1, 0.5, -0.8, 1, 0, 0)]
    [InlineData(-1, -0.2, 0.9, -1, 0, 0)]
    [InlineData(-0.4, 1, -0.1, 0, 1, 0)]
    [InlineData(0.3, -1, -0.7, 0, -1, 0)]
    [InlineData(-0.6, 0.3, 1, 0, 0, 1)]
    [InlineData(0.4, 0.4, -1, 0, 0, -1)]
    [InlineData(1, 1, 1, 1, 0, 0)]
    [InlineData(-1, -1, -1, -1, 0, 0)]
    public void NormalOnSurfaceOfCube(double x, double y, double z,
        double nx, double ny, double nz)
    {
        var c = new Cube();
        var p = Tuple.CreatePoint(x, y, z);

        var result = c.NormalAt(p);
        var expectedResult = Tuple.CreateVector(nx, ny, nz);
        
        Assert.Equal(expectedResult, result);
    }
    
    [Fact]
    public void BoundsOfACube()
    {
        var s = new Sphere();
        
        var result = s.GetBounds();
        var expected = new Bounds(-1, -1, -1, 1, 1, 1);
        
        Assert.Equal(expected, result);
    }
}