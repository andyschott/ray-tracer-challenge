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
        Assert.Equal(0.1M, m.Ambient);
        Assert.Equal(0.9M, m.Diffuse);
        Assert.Equal(0.9M, m.Specular);
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
        var expectedResult = new Color(1.9M, 1.9M, 1.9M);
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void LightingWithEyeBetweenLightAndSurface_EyeOffset()
    {
        var eyeVector = Tuple.CreateVector(0,
            (decimal)Math.Sqrt(2)/2, -(decimal)Math.Sqrt(2)/2);
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
        var expectedResult = new Color(0.7364M, 0.7364M, 0.7364M);
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void LightingWithEyeInPathOfReflection()
    {
        var eyeVector = Tuple.CreateVector(0,
            -(decimal)Math.Sqrt(2)/2, -(decimal)Math.Sqrt(2)/2);
        var normalVector = Tuple.CreateVector(0, 0, -1);
        var light = new PointLight(Tuple.CreatePoint(0, 10, -10),
            new Color(1, 1, 1));
        
        var result = _material.Lighting(light, _position,
            eyeVector, normalVector);
        var expectedResult = new Color(1.6364M, 1.6364M, 1.6364M);
        
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
        var expectedResult = new Color(0.1M, 0.1M, 0.1M);
        
        Assert.Equal(expectedResult, result);
    }
}