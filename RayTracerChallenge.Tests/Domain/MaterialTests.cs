using RayTracerChallenge.Domain;
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
    }

    [Fact]
    public void LightingWithEyeBetweenLightAndSurface()
    {
        var eyeVector = Tuple.CreateVector(0, 0, -1);
        var normalVector = Tuple.CreateVector(0, 0, -1);
        var light = new PointLight(Tuple.CreatePoint(0, 0, -10),
            new Color(1, 1, 1));
        
        var result = _material.Lighting(light, _position,
            eyeVector, normalVector);
        var expectedResult = new Color(1.9, 1.9, 1.9);
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void LightingWithEyeBetweenLightAndSurface_EyeOffset()
    {
        var eyeVector = Tuple.CreateVector(0,
            Math.Sqrt(2)/2, -Math.Sqrt(2)/2);
        var normalVector = Tuple.CreateVector(0, 0, -1);
        var light = new PointLight(Tuple.CreatePoint(0, 0, -10),
            new Color(1, 1, 1));
        
        var result = _material.Lighting(light, _position,
            eyeVector, normalVector);
        var expectedResult = new Color(1, 1, 1);
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void LightingWithEyeOppositeSurface_LightOffset()
    {
        var eyeVector = Tuple.CreateVector(0, 0, -1);
        var normalVector = Tuple.CreateVector(0, 0, -1);
        var light = new PointLight(Tuple.CreatePoint(0, 10, -10),
            new Color(1, 1, 1));
        
        var result = _material.Lighting(light, _position,
            eyeVector, normalVector);
        var expectedResult = new Color(0.7364, 0.7364, 0.7364);
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void LightingWithEyeInPathOfReflection()
    {
        var eyeVector = Tuple.CreateVector(0,
            -Math.Sqrt(2)/2, -Math.Sqrt(2)/2);
        var normalVector = Tuple.CreateVector(0, 0, -1);
        var light = new PointLight(Tuple.CreatePoint(0, 10, -10),
            new Color(1, 1, 1));
        
        var result = _material.Lighting(light, _position,
            eyeVector, normalVector);
        var expectedResult = new Color(1.6364, 1.6364, 1.6364);
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void LightingWithLightBehindSurface()
    {
        var eyeVector = Tuple.CreateVector(0, 0, -1);
        var normalVector = Tuple.CreateVector(0, 0, -1);
        var light = new PointLight(Tuple.CreatePoint(0, 0, 10),
            new Color(1, 1, 1));
        
        var result = _material.Lighting(light, _position,
            eyeVector, normalVector);
        var expectedResult = new Color(0.1, 0.1, 0.1);
        
        Assert.Equal(expectedResult, result);
    }
}