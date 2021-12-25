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
}
