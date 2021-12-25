using System.Linq;

namespace RayTracerChallenge.Domain.Tests;

public class SphereTests
{
    [Fact]
    public void RayIntersectsSphereTwice()
    {
        var ray = new Ray(Tuple.CreatePoint(0, 0, -5), Tuple.CreateVector(0, 0, 1));
        var sphere = new Sphere();

        var result = sphere.Intersects(ray);

        var expectedResult = new[] { 4.0M, 6.0M };
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void RayIntersectsSphereAtTangent()
    {
        var ray = new Ray(Tuple.CreatePoint(0, 1, -5), Tuple.CreateVector(0, 0, 1));
        var sphere = new Sphere();

        var result = sphere.Intersects(ray);

        var expectedResult = Enumerable.Repeat(5.0M, 2);
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void RayMissedSphere()
    {
        var ray = new Ray(Tuple.CreatePoint(0, 2, -5), Tuple.CreateVector(0, 0, 1));
        var sphere = new Sphere();

        var result = sphere.Intersects(ray);

        Assert.Empty(result);
    }

    [Fact]
    public void RayStartsInsideSphere()
    {
        var ray = new Ray(Tuple.CreatePoint(0, 0, 0), Tuple.CreateVector(0, 0, 1));
        var sphere = new Sphere();

        var result = sphere.Intersects(ray);

        var expectedResult = new[] { -1.0M, 1.0M };
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void SphereIsBehindRay()
    {
        var ray = new Ray(Tuple.CreatePoint(0, 0, 5), Tuple.CreateVector(0, 0, 1));
        var sphere = new Sphere();

        var result = sphere.Intersects(ray);

        var expectedResult = new[] { -6.0M, -4.0M };
        Assert.Equal(expectedResult, result);
    }
}
