using System;
using System.Collections.Generic;

namespace RayTracerChallenge.Domain.Tests;

public sealed class MaterialComparerTests : IDisposable
{
    private readonly IFixture _fixture = new Fixture();
    private readonly MockRepository _repository = new MockRepository(MockBehavior.Strict);

    private readonly Mock<IEqualityComparer<Color>> _mockColorComparer;

    private readonly MaterialComparer _comparer;

    public MaterialComparerTests()
    {
        _mockColorComparer = _repository.Create<IEqualityComparer<Color>>();
        _comparer = new MaterialComparer(_mockColorComparer.Object);
    }

    public void Dispose()
    {
        _repository.Verify();
        _repository.VerifyNoOtherCalls();
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
        var color = _fixture.Create<Color>();
        var ambient = _fixture.Create<decimal>();
        var diffuse = _fixture.Create<decimal>();
        var specular = _fixture.Create<decimal>();
        var shininess = _fixture.Create<decimal>();

        var x = new Material
        {
            Color = color,
            Ambient = ambient,
            Diffuse = diffuse,
            Specular = specular,
            Shininess = shininess
        };
        var y = new Material
        {
            Color = color,
            Ambient = ambient,
            Diffuse = diffuse,
            Specular = specular,
            Shininess = shininess
        };

        _mockColorComparer.Setup(comparer => comparer.Equals(x.Color, y.Color))
            .Returns(true)
            .Verifiable();

        Assert.True(_comparer.Equals(x, y));
    }

    [Fact]
    public void ColorsAreDifferent()
    {
        var ambient = _fixture.Create<decimal>();
        var diffuse = _fixture.Create<decimal>();
        var specular = _fixture.Create<decimal>();
        var shininess = _fixture.Create<decimal>();

        var x = _fixture.Build<Material>()
            .With(m => m.Ambient, ambient)
            .With(m => m.Diffuse, diffuse)
            .With(m => m.Specular, specular)
            .With(m => m.Shininess, shininess)
            .Create();
        var y = _fixture.Build<Material>()
            .With(m => m.Ambient, ambient)
            .With(m => m.Diffuse, diffuse)
            .With(m => m.Specular, specular)
            .With(m => m.Shininess, shininess)
            .Create();

        _mockColorComparer.Setup(comparer => comparer.Equals(x.Color, y.Color))
            .Returns(false)
            .Verifiable();

        Assert.False(_comparer.Equals(x, y));
    }

    [Fact]
    public void AmbientIsDifferent()
    {
        var color = _fixture.Create<Color>();
        var diffuse = _fixture.Create<decimal>();
        var specular = _fixture.Create<decimal>();
        var shininess = _fixture.Create<decimal>();

        var x = _fixture.Build<Material>()
            .With(m => m.Color, color)
            .With(m => m.Diffuse, diffuse)
            .With(m => m.Specular, specular)
            .With(m => m.Shininess, shininess)
            .Create();
        var y = _fixture.Build<Material>()
            .With(m => m.Color, color)
            .With(m => m.Diffuse, diffuse)
            .With(m => m.Specular, specular)
            .With(m => m.Shininess, shininess)
            .Create();

        _mockColorComparer.Setup(comparer => comparer.Equals(x.Color, y.Color))
            .Returns(false)
            .Verifiable();

        Assert.False(_comparer.Equals(x, y));
    }

    [Fact]
    public void DiffuseIsDifferent()
    {
        var color = _fixture.Create<Color>();
        var ambient = _fixture.Create<decimal>();
        var specular = _fixture.Create<decimal>();
        var shininess = _fixture.Create<decimal>();

        var x = _fixture.Build<Material>()
            .With(m => m.Color, color)
            .With(m => m.Ambient, ambient)
            .With(m => m.Specular, specular)
            .With(m => m.Shininess, shininess)
            .Create();
        var y = _fixture.Build<Material>()
            .With(m => m.Color, color)
            .With(m => m.Ambient, ambient)
            .With(m => m.Specular, specular)
            .With(m => m.Shininess, shininess)
            .Create();

        _mockColorComparer.Setup(comparer => comparer.Equals(x.Color, y.Color))
            .Returns(false)
            .Verifiable();

        Assert.False(_comparer.Equals(x, y));
    }

    [Fact]
    public void SpecularIsDifferent()
    {
        var color = _fixture.Create<Color>();
        var ambient = _fixture.Create<decimal>();
        var diffuse = _fixture.Create<decimal>();
        var shininess = _fixture.Create<decimal>();

        var x = _fixture.Build<Material>()
            .With(m => m.Color, color)
            .With(m => m.Ambient, ambient)
            .With(m => m.Diffuse, diffuse)
            .With(m => m.Shininess, shininess)
            .Create();
        var y = _fixture.Build<Material>()
            .With(m => m.Color, color)
            .With(m => m.Ambient, ambient)
            .With(m => m.Diffuse, diffuse)
            .With(m => m.Shininess, shininess)
            .Create();

        _mockColorComparer.Setup(comparer => comparer.Equals(x.Color, y.Color))
            .Returns(false)
            .Verifiable();

        Assert.False(_comparer.Equals(x, y));
    }

    [Fact]
    public void ShininessIsDifferent()
    {
        var color = _fixture.Create<Color>();
        var ambient = _fixture.Create<decimal>();
        var diffuse = _fixture.Create<decimal>();
        var specular = _fixture.Create<decimal>();

        var x = _fixture.Build<Material>()
            .With(m => m.Color, color)
            .With(m => m.Ambient, ambient)
            .With(m => m.Diffuse, diffuse)
            .With(m => m.Specular, specular)
            .Create();
        var y = _fixture.Build<Material>()
            .With(m => m.Color, color)
            .With(m => m.Ambient, ambient)
            .With(m => m.Diffuse, diffuse)
            .With(m => m.Specular, specular)
            .Create();

        _mockColorComparer.Setup(comparer => comparer.Equals(x.Color, y.Color))
            .Returns(false)
            .Verifiable();

        Assert.False(_comparer.Equals(x, y));
    }
}
