using System.Linq;

namespace RayTracerChallenge.Domain.Tests;

public class SphereTests
{
    private readonly IntersectionComparer _intersectionComparer = new IntersectionComparer();

    [Fact]
    public void RayIntersectsSphereTwice()
    {
        var ray = new Ray(Tuple.CreatePoint(0, 0, -5), Tuple.CreateVector(0, 0, 1));
        var sphere = new Sphere();

        var result = sphere.Intersects(ray);

        var expectedResult = new[]
        {
            new Intersection(4.0M, sphere),
            new Intersection(6.0M, sphere)
        };
        Assert.Equal(expectedResult, result, _intersectionComparer);
    }

    [Fact]
    public void RayIntersectsSphereAtTangent()
    {
        var ray = new Ray(Tuple.CreatePoint(0, 1, -5), Tuple.CreateVector(0, 0, 1));
        var sphere = new Sphere();

        var result = sphere.Intersects(ray);

        var expectedResult = Enumerable.Repeat(new Intersection(5.0M, sphere), 2);
        Assert.Equal(expectedResult, result, _intersectionComparer);
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

        var expectedResult = new[]
        {
            new Intersection(-1.0M, sphere),
            new Intersection(1.0M, sphere)
        };
        Assert.Equal(expectedResult, result, _intersectionComparer);
    }

    [Fact]
    public void SphereIsBehindRay()
    {
        var ray = new Ray(Tuple.CreatePoint(0, 0, 5), Tuple.CreateVector(0, 0, 1));
        var sphere = new Sphere();

        var result = sphere.Intersects(ray);

        var expectedResult = new[]
        {
            new Intersection(-6.0M, sphere),
            new Intersection(-4.0M, sphere)
        };
        Assert.Equal(expectedResult, result, _intersectionComparer);
    }
}
