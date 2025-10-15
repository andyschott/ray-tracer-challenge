using RayTracerChallenge.Domain;
using RayTracerChallenge.Extensions;
using Tuple = RayTracerChallenge.Domain.Tuple;

namespace RayTracerChallenge.Tests.Domain;

public class IntersectionsTests
{
    [Fact]
    public void AggregatingIntersections()
    {
        var s = new Sphere();
        var i1 = new Intersection(1, s);
        var i2 = new Intersection(2, s);

        var xs = new Intersections
        {
            i1,
            i2
        };
        
        Assert.Equal([i1, i2], xs);
    }

    [Fact]
    public void Hit_AllIntersectionsHavePositiveT()
    {
        var s = new Sphere();
        var xs = new Intersections
        {
            new Intersection(2, s),
            new Intersection(1, s),
        };

        var i = xs.Hit();
        
        Assert.Same(xs[1], i);
    }

    [Fact]
    public void Hit_SomeIntersectionsHaveNegativeT()
    {
        var s = new Sphere();
        var xs = new Intersections
        {
            new Intersection(1, s),
            new Intersection(-1, s),
        };

        var i = xs.Hit();

        Assert.Same(xs[0], i);
    }

    [Fact]
    public void Hit_AllIntersectionsHaveNegativeT()
    {
        var s = new Sphere();
        var xs = new Intersections
        {
            new Intersection(-1, s),
            new Intersection(-2, s),
        };

        var i = xs.Hit();

        Assert.Null(i);
    }

    [Fact]
    public void HitIsAlwaysLowestNonNegativeIntersection()
    {
        var s = new Sphere();
        var xs = new Intersections
        {
            new Intersection(5, s),
            new Intersection(7, s),
            new Intersection(-3, s),
            new Intersection(2, s),
        };

        var i = xs.Hit();

        Assert.Same(xs[3], i);
    }
}