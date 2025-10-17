using RayTracerChallenge.Domain;
using RayTracerChallenge.Domain.Patterns;
using RayTracerChallenge.Domain.Shapes;
using Tuple = RayTracerChallenge.Domain.Tuple;

namespace RayTracerChallenge.Tests.Domain;

public class MaterialTests
{
    private readonly Material _material = new();
    private readonly Tuple _position = Tuple.CreatePoint(0, 0, 0);
    
    [Fact]
    public void DefaultMaterial()
    {
        var m = new Material();
        
        Assert.Equal(new Color(1, 1, 1), m.Color);
        Assert.Equal(0.1, m.Ambient);
        Assert.Equal(0.9, m.Diffuse);
        Assert.Equal(0.9, m.Specular);
        Assert.Equal(200, m.Shininess);
        Assert.Equal(0, m.Reflective);
        Assert.Equal(0, m.Transparency);
        Assert.Equal(1, m.RefractiveIndex);
    }

    [Fact]
    public void LightingWithEyeBetweenLightAndSurface()
    {
        var s = new Sphere();
        var eyeVector = Tuple.CreateVector(0, 0, -1);
        var normalVector = Tuple.CreateVector(0, 0, -1);
        var light = new PointLight(Tuple.CreatePoint(0, 0, -10),
            new Color(1, 1, 1));
        
        var result = _material.Lighting(s, light, _position,
            eyeVector, normalVector);
        var expectedResult = new Color(1.9, 1.9, 1.9);
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void LightingWithEyeBetweenLightAndSurface_EyeOffset()
    {
        var s = new Sphere();
        var eyeVector = Tuple.CreateVector(0,
            Math.Sqrt(2)/2, -Math.Sqrt(2)/2);
        var normalVector = Tuple.CreateVector(0, 0, -1);
        var light = new PointLight(Tuple.CreatePoint(0, 0, -10),
            new Color(1, 1, 1));
        
        var result = _material.Lighting(s, light, _position,
            eyeVector, normalVector);
        var expectedResult = new Color(1, 1, 1);
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void LightingWithEyeOppositeSurface_LightOffset()
    {
        var s = new Sphere();
        var eyeVector = Tuple.CreateVector(0, 0, -1);
        var normalVector = Tuple.CreateVector(0, 0, -1);
        var light = new PointLight(Tuple.CreatePoint(0, 10, -10),
            new Color(1, 1, 1));
        
        var result = _material.Lighting(s, light, _position,
            eyeVector, normalVector);
        var expectedResult = new Color(0.7364, 0.7364, 0.7364);
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void LightingWithEyeInPathOfReflection()
    {
        var s = new Sphere();
        var eyeVector = Tuple.CreateVector(0,
            -Math.Sqrt(2)/2, -Math.Sqrt(2)/2);
        var normalVector = Tuple.CreateVector(0, 0, -1);
        var light = new PointLight(Tuple.CreatePoint(0, 10, -10),
            new Color(1, 1, 1));
        
        var result = _material.Lighting(s, light, _position,
            eyeVector, normalVector);
        var expectedResult = new Color(1.6364, 1.6364, 1.6364);
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void LightingWithLightBehindSurface()
    {
        var s = new Sphere();
        var eyeVector = Tuple.CreateVector(0, 0, -1);
        var normalVector = Tuple.CreateVector(0, 0, -1);
        var light = new PointLight(Tuple.CreatePoint(0, 0, 10),
            new Color(1, 1, 1));
        
        var result = _material.Lighting(s, light, _position,
            eyeVector, normalVector);
        var expectedResult = new Color(0.1, 0.1, 0.1);
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void LightingWithSurfaceInShadow()
    {
        var s = new Sphere();
        var eyeVector = Tuple.CreateVector(0, 0, -1);
        var normalVector = Tuple.CreateVector(0, 0, -1);
        var light = new PointLight(Tuple.CreatePoint(0, 0, -10),
            new Color(1, 1, 1));
        
        var result = _material.Lighting(s, light, _position,
            eyeVector, normalVector, true);
        var expectedResult = new Color(0.1, 0.1, 0.1);
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void LightingWithPatternApplied()
    {
        var s = new Sphere();
        var colorA = new Color(1, 1, 1);
        var colorB = new Color(0, 0, 0);
        var m = new Material
        {
            Pattern = new StripePattern(colorA,
                colorB),
            Ambient = 1,
            Diffuse = 0,
            Specular = 0
        };
        var eyeVector = Tuple.CreateVector(0, 0, -1);
        var normalVector = Tuple.CreateVector(0, 0, -1);
        var light = new PointLight(Tuple.CreatePoint(0, 0, -10),
            new Color(1, 1, 1));
        
        var result = m.Lighting(s, light, Tuple.CreatePoint(0.9, 0, 0),
            eyeVector, normalVector);
        Assert.Equal(colorA, result);
        
        result = m.Lighting(s, light, Tuple.CreatePoint(1.1, 0, 0),
            eyeVector, normalVector);
        Assert.Equal(colorB, result);
    }
}