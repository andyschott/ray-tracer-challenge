using System;
using RayTracerChallenge.Domain.Patterns;

namespace RayTracerChallenge.Domain.Tests;

public class MaterialComparerTests
{
    private readonly IFixture _fixture = new Fixture();

    private readonly MaterialComparer _comparer = new MaterialComparer();

    public MaterialComparerTests()
    {
        _fixture.Customize<Material>(c => c.Without(m => m.Pattern));
    }

    [Fact]
    public void BothObjectsNull()
    {
        var result = _comparer.Equals(null, null);

        Assert.True(result);
    }

    [Fact]
    public void FirstObjectNull()
    {
        var result = _comparer.Equals(null, _fixture.Create<Material>());

        Assert.False(result);
    }

    [Fact]
    public void SecondObjectNull()
    {
        var result = _comparer.Equals(_fixture.Create<Material>(), null);

        Assert.False(result);
    }

    [Fact]
    public void ObjectsAreEqual()
    {
        var ambient = _fixture.Create<decimal>();
        var diffuse = _fixture.Create<decimal>();
        var specular = _fixture.Create<decimal>();
        var shininess = _fixture.Create<decimal>();
        var pattern = _fixture.Create<SolidPattern>();

        var x = new Material
        {
            Ambient = ambient,
            Diffuse = diffuse,
            Specular = specular,
            Shininess = shininess,
            Pattern = pattern
        };
        var y = new Material
        {
            Ambient = ambient,
            Diffuse = diffuse,
            Specular = specular,
            Shininess = shininess,
            Pattern = pattern
        };

        Assert.True(_comparer.Equals(x, y));
    }

    [Fact]
    public void AmbientIsDifferent()
    {
        var diffuse = _fixture.Create<decimal>();
        var specular = _fixture.Create<decimal>();
        var shininess = _fixture.Create<decimal>();
        var pattern = _fixture.Create<SolidPattern>();

        var x = _fixture.Build<Material>()
            .With(m => m.Diffuse, diffuse)
            .With(m => m.Specular, specular)
            .With(m => m.Shininess, shininess)
            .With(m => m.Pattern, pattern)
            .Create();
        var y = _fixture.Build<Material>()
            .With(m => m.Diffuse, diffuse)
            .With(m => m.Specular, specular)
            .With(m => m.Shininess, shininess)
            .With(m => m.Pattern, pattern)
            .Create();

        Assert.False(_comparer.Equals(x, y));
    }

    [Fact]
    public void DiffuseIsDifferent()
    {
        var ambient = _fixture.Create<decimal>();
        var specular = _fixture.Create<decimal>();
        var shininess = _fixture.Create<decimal>();
        var pattern = _fixture.Create<SolidPattern>();

        var x = _fixture.Build<Material>()
            .With(m => m.Ambient, ambient)
            .With(m => m.Specular, specular)
            .With(m => m.Shininess, shininess)
            .With(m => m.Pattern, pattern)
            .Create();
        var y = _fixture.Build<Material>()
            .With(m => m.Ambient, ambient)
            .With(m => m.Specular, specular)
            .With(m => m.Shininess, shininess)
            .With(m => m.Pattern, pattern)
            .Create();

        Assert.False(_comparer.Equals(x, y));
    }

    [Fact]
    public void SpecularIsDifferent()
    {
        var ambient = _fixture.Create<decimal>();
        var diffuse = _fixture.Create<decimal>();
        var shininess = _fixture.Create<decimal>();
        var pattern = _fixture.Create<SolidPattern>();

        var x = _fixture.Build<Material>()
            .With(m => m.Ambient, ambient)
            .With(m => m.Diffuse, diffuse)
            .With(m => m.Shininess, shininess)
            .With(m => m.Pattern, pattern)
            .Create();
        var y = _fixture.Build<Material>()
            .With(m => m.Ambient, ambient)
            .With(m => m.Diffuse, diffuse)
            .With(m => m.Shininess, shininess)
            .With(m => m.Pattern, pattern)
            .Create();

        Assert.False(_comparer.Equals(x, y));
    }

    [Fact]
    public void ShininessIsDifferent()
    {
        var ambient = _fixture.Create<decimal>();
        var diffuse = _fixture.Create<decimal>();
        var specular = _fixture.Create<decimal>();
        var pattern = _fixture.Create<SolidPattern>();

        var x = _fixture.Build<Material>()
            .With(m => m.Ambient, ambient)
            .With(m => m.Diffuse, diffuse)
            .With(m => m.Specular, specular)
            .With(m => m.Pattern, pattern)
            .Create();
        var y = _fixture.Build<Material>()
            .With(m => m.Ambient, ambient)
            .With(m => m.Diffuse, diffuse)
            .With(m => m.Specular, specular)
            .With(m => m.Pattern, pattern)
            .Create();

        Assert.False(_comparer.Equals(x, y));
    }
}
