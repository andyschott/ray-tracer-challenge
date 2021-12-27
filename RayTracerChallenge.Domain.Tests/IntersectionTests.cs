using RayTracerChallenge.Domain.Extensions;

namespace RayTracerChallenge.Domain.Tests;

public class IntersectionTests
{
    [Fact]
    public void IntersectionEncapsulatesTAndObject()
    {
        var sphere = new Sphere();
        var t = 3.5M;

        var intersection = new Intersection(t, sphere);

        Assert.Equal(t, intersection.T);
        Assert.Same(sphere, intersection.Object);
    }

    [Fact]
    public void FindHitInIntersectionsWithPositiveT()
    {
        var sphere = new Sphere();
        var intersection1 = new Intersection(1, sphere);
        var intersection2 = new Intersection(2, sphere);
        var intersections = new[] { intersection1, intersection2 };

        var result = intersections.Hit();

        Assert.Same(intersection1, result);
    }

    [Fact]
    public void FindHitInIntersectionsWithSomeNegativeT()
    {
        var sphere = new Sphere();
        var intersection1 = new Intersection(-1, sphere);
        var intersection2 = new Intersection(1, sphere);
        var intersections = new[] { intersection1, intersection2 };

        var result = intersections.Hit();

        Assert.Same(intersection2, result);
    }

    [Fact]
    public void NoHitInIntersections()
    {
        var sphere = new Sphere();
        var intersection1 = new Intersection(-2, sphere);
        var intersection2 = new Intersection(-1, sphere);
        var intersections = new[] { intersection1, intersection2 };

        var result = intersections.Hit();

        Assert.Null(result);
    }

    [Fact]
    public void HitIsLowestNonNegativeIntersection()
    {
        var sphere = new Sphere();
        var intersection1 = new Intersection(5, sphere);
        var intersection2 = new Intersection(7, sphere);
        var intersection3 = new Intersection(-3, sphere);
        var intersection4 = new Intersection(2, sphere);
        var intersections = new[]
        {
            intersection1,
            intersection2,
            intersection3,
            intersection4
        };

        var result = intersections.Hit();

        Assert.Same(intersection4, result);
    }
}
