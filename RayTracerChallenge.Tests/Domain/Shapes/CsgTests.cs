using RayTracerChallenge.Domain;
using RayTracerChallenge.Domain.Shapes;
using RayTracerChallenge.Extensions;

namespace RayTracerChallenge.Tests.Domain.Shapes;

public class CsgTests
{
    [Fact]
    public void ConstructedSuccessfully()
    {
        var s1 = new Sphere();
        var s2 = new Cube();
        
        var result = new Csg(CsgOperation.Union, s1, s2);
        
        Assert.Equal(CsgOperation.Union, result.Operation);
        Assert.Same(s1, result.Left);
        Assert.Same(s2, result.Right);
        Assert.Same(result, s1.Parent);
        Assert.Same(result, s2.Parent);
    }

    [Theory]
    [InlineData(CsgOperation.Union, true, true, true, false)]
    [InlineData(CsgOperation.Union, true, true, false, true)]
    [InlineData(CsgOperation.Union, true, false, true, false)]
    [InlineData(CsgOperation.Union, true, false, false, true)]
    [InlineData(CsgOperation.Union, false, true, true, false)]
    [InlineData(CsgOperation.Union, false, true, false, false)]
    [InlineData(CsgOperation.Union, false, false, true, true)]
    [InlineData(CsgOperation.Union, false, false, false, true)]
    [InlineData(CsgOperation.Intersection, true, true, true, true)]
    [InlineData(CsgOperation.Intersection, true, true, false, false)]
    [InlineData(CsgOperation.Intersection, true, false, true, true)]
    [InlineData(CsgOperation.Intersection, true, false, false, false)]
    [InlineData(CsgOperation.Intersection, false, true, true, true)]
    [InlineData(CsgOperation.Intersection, false, true, false, true)]
    [InlineData(CsgOperation.Intersection, false, false, true, false)]
    [InlineData(CsgOperation.Intersection, false, false, false, false)]
    [InlineData(CsgOperation.Difference, true, true, true, false)]
    [InlineData(CsgOperation.Difference, true, true, false, true)]
    [InlineData(CsgOperation.Difference, true, false, true, false)]
    [InlineData(CsgOperation.Difference, true, false, false, true)]
    [InlineData(CsgOperation.Difference, false, true, true, true)]
    [InlineData(CsgOperation.Difference, false, true, false, true)]
    [InlineData(CsgOperation.Difference, false, false, true, false)]
    [InlineData(CsgOperation.Difference, false, false, false, false)]
    public void EvaluatingCsgOperationRules(CsgOperation operation,
        bool leftHit,
        bool isHitInsideLeft,
        bool isHitInsideRight,
        bool expectedResult)
    {
        var s1 = new Sphere();
        var s2 = new Cube();
        var csg = new Csg(operation, s1, s2);
        
        var result = csg.IntersectionAllowed(leftHit, isHitInsideLeft, isHitInsideRight);
        
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(CsgOperation.Union, 0, 3)]
    [InlineData(CsgOperation.Intersection, 1, 2)]
    [InlineData(CsgOperation.Difference, 0, 1)]
    public void FilterIntersections(CsgOperation operation, int x0, int x1)
    {
        var s1 = new Sphere();
        var s2 = new Cube();
        var csg =  new Csg(operation, s1, s2);
        var xs = new Intersections
        {
            new Intersection(1, s1),
            new Intersection(2, s2),
            new Intersection(3, s1),
            new Intersection(4, s2)
        };
        
        var result = csg.FilterIntersections(xs);
        
        Assert.Equal(2, result.Count);
        Assert.Same(xs[x0], result[0]);
        Assert.Same(xs[x1], result[1]);
    }

    [Fact]
    public void RayMissesCsg()
    {
        var c = new Csg(CsgOperation.Union,
            new Sphere(),
            new Cube());
        var r = new Ray(0, 2, -5, 0, 0, 1);

        var result = c.Intersects(r);
        
        Assert.Empty(result);
    }

    [Fact]
    public void RayHitCsg()
    {
        var s1 = new Sphere();
        var s2 = new Sphere(Matrix.Identity.Translate(0, 0, 0.5));
        var c = new Csg(CsgOperation.Union, s1, s2);
        var r = new Ray(0, 0, -5, 0, 0, 1);
        
        var result = c.Intersects(r);
        
        Assert.Equal(2, result.Count);
        Assert.Equal(4, result[0].T);
        Assert.Same(s1, result[0].Shape);
        Assert.Equal(6.5,  result[1].T);
        Assert.Same(s2, result[1].Shape);
    }
}