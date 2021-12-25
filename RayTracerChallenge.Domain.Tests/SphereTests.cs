using System.Linq;

namespace RayTracerChallenge.Domain.Tests;

public class SphereTests
{
    private readonly TransformationFactory _factory = new TransformationFactory();

    private readonly IntersectionComparer _intersectionComparer = new IntersectionComparer();
    private readonly MatrixComparer _matrixComparer = new MatrixComparer();

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

    [Fact]
    public void SphereDefaultTransformation()
    {
        var sphere = new Sphere();

        var expectedResult = Matrix.Identity();
        Assert.Equal(expectedResult, sphere.Transform, _matrixComparer);
    }

    [Fact]
    public void ChangeSphereTransformation()
    {
        var sphere = new Sphere();
        var transform = _factory.Translation(2, 3, 4);

        sphere.Transform = transform;

        Assert.Same(transform, sphere.Transform);
    }

    [Fact]
    public void IntersectingScaledSphereWithRay()
    {
        var ray = new Ray(Tuple.CreatePoint(0, 0, -5), Tuple.CreateVector(0, 0, 1));
        var sphere = new Sphere()
        {
            Transform = _factory.Scale(2, 2, 2)
        };

        var result = sphere.Intersects(ray);

        var expectedResult = new[]
        {
            new Intersection(3, sphere),
            new Intersection(7, sphere)
        };
        Assert.Equal(expectedResult, result, _intersectionComparer);
    }
}
