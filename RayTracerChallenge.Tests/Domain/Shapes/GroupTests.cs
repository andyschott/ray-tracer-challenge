using RayTracerChallenge.Domain;
using RayTracerChallenge.Domain.Shapes;
using RayTracerChallenge.Extensions;
using Tuple = RayTracerChallenge.Domain.Tuple;

namespace RayTracerChallenge.Tests.Domain.Shapes;

public class GroupTests
{
    [Fact]
    public void CreateGroup()
    {
        var g = new Group();
        
        Assert.Equal(Matrix.Identity, g.Transform);
        Assert.Empty(g);
    }

    [Fact]
    public void AddChildToGroup()
    {
        var g = new Group();
        var s = new Sphere();
        
        g.Add(s);
        var child = Assert.Single(g);
        Assert.Same(s, child);
        Assert.Same(g, child.Parent);
    }

    [Fact]
    public void IntersectingRayWithEmptyGroup()
    {
        var g = new Group();
        var r = new Ray(0, 0, 0, 0, 0, 1);

        var result = g.Intersects(r);
        
        Assert.Empty(result);
    }

    [Fact]
    public void IntersectingRayWithNonEmptyGroup()
    {
        var g = new Group();
        var s1 = new Sphere();
        g.Add(s1);
        var s2 = new Sphere(Matrix.Identity
            .Translate(0, 0, -3));
        g.Add(s2);
        var s3 = new Sphere(Matrix.Identity
            .Translate(5, 0, 0));
        g.Add(s3);
        var r = new Ray(0, 0, -5, 0, 0, 1);
        
        var result = g.Intersects(r);

        Assert.Equal(4, result.Count);
        Assert.Same(s2, result[0].Shape);
        Assert.Same(s2, result[1].Shape);
        Assert.Same(s1, result[2].Shape);
        Assert.Same(s1, result[3].Shape);
    }

    [Fact]
    public void IntersectingTransformedGroup()
    {
        var g = new Group(Matrix.Identity.Scale(2, 2, 2));
        var s = new Sphere(Matrix.Identity.Translate(5, 0, 0));
        g.Add(s);
        var r = new Ray(10, 0, -10, 0, 0, 1);
        
        var result = g.Intersects(r);
        Assert.Equal(2, result.Count);
    }
    
    [Fact]
    public void ConvertPointFromWorldToObjectSpace()
    {
        var g1 = new Group(Matrix.Identity.RotateY(Math.PI / 2));
        var g2 = new Group(Matrix.Identity.Scale(2, 2, 2));
        g1.Add(g2);
        var s = new Sphere(Matrix.Identity.Translate(5, 0, 0));
        g2.Add(s);

        var result = s.ConvertToObjectSpace(Tuple.CreatePoint(-2, 0, -10));
        var expectedResult = Tuple.CreatePoint(0, 0, -1);
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void ConvertNormalFromWorldToObjectSpace()
    {
        var g1 = new Group(Matrix.Identity.RotateY(Math.PI / 2));
        var g2 = new Group(Matrix.Identity.Scale(1, 2, 3));
        g1.Add(g2);
        var s = new Sphere(Matrix.Identity.Translate(5, 0, 0));
        g2.Add(s);
        
        var result = s.ConvertNormalToWorld(Tuple.CreateVector(Math.Sqrt(3)/3,
            Math.Sqrt(3)/3,
            Math.Sqrt(3)/3));
        var expectedResult = Tuple.CreateVector(0.2857, 0.4286, -0.8571);
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void FindNormalOfChildObject()
    {
        var g1 = new Group(Matrix.Identity.RotateY(Math.PI / 2));
        var g2 = new Group(Matrix.Identity.Scale(1, 2, 3));
        g1.Add(g2);
        var s = new Sphere(Matrix.Identity.Translate(5, 0, 0));
        g2.Add(s);

        var result = s.NormalAt(Tuple.CreatePoint(1.7321, 1.1547, -5.5774));
        var expectedResult = Tuple.CreateVector(0.2857, 0.4286, -0.8571);
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void BoundsOfAGroup()
    {
        var g = new Group();
        var c = new Cube(Matrix.Identity.Translate(5, 5, 5));
        g.Add(c);
        var s = new Sphere(Matrix.Identity.Translate(-5, -5, -5));
        g.Add(s);

        var result = g.GetBounds();
        var expectedResult = new Bounds(-6, -6, -6, 6, 6, 6);
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void BoundsOfATransformedGroup()
    {
        var g1 = new Group();
        var g2 = new Group(Matrix.Identity.Scale(2, 2, 2));
        g1.Add(g2);
        var c = new Cube(Matrix.Identity.Translate(5, 5, 5));
        g2.Add(c);
        var s = new Sphere(Matrix.Identity.Translate(-5, -5, -5));
        g2.Add(s);
        
        var result = g1.GetBounds();
        var expectedResult = new Bounds(-12, -12, -12, 12, 12, 12);
        
        Assert.Equal(expectedResult, result);
    }
}