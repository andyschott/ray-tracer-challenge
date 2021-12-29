using System;
using System.Collections.Generic;
using RayTracerChallenge.Domain.Patterns;

namespace RayTracerChallenge.Domain.Tests;

public sealed class MaterialComparerTests : IDisposable
{
    private readonly IFixture _fixture = new Fixture();
    private readonly MockRepository _repository = new MockRepository(MockBehavior.Strict);

    private readonly Mock<IEqualityComparer<Color>> _mockColorComparer;

    private readonly MaterialComparer _comparer;

    public MaterialComparerTests()
    {
        _fixture.Customize<Material>(c => c.Without(m => m.Pattern));

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
        var pattern = _fixture.Create<StripePattern>();

        var x = new Material
        {
            Color = color,
            Ambient = ambient,
            Diffuse = diffuse,
            Specular = specular,
            Shininess = shininess,
            Pattern = pattern
        };
        var y = new Material
        {
            Color = color,
            Ambient = ambient,
            Diffuse = diffuse,
            Specular = specular,
            Shininess = shininess,
            Pattern = pattern
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
            .Without(m => m.Pattern)
            .Create();
        var y = _fixture.Build<Material>()
            .With(m => m.Ambient, ambient)
            .With(m => m.Diffuse, diffuse)
            .With(m => m.Specular, specular)
            .With(m => m.Shininess, shininess)
            .Without(m => m.Pattern)
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
            .Without(m => m.Pattern)
            .Create();
        var y = _fixture.Build<Material>()
            .With(m => m.Color, color)
            .With(m => m.Diffuse, diffuse)
            .With(m => m.Specular, specular)
            .With(m => m.Shininess, shininess)
            .Without(m => m.Pattern)
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
            .Without(m => m.Pattern)
            .Create();
        var y = _fixture.Build<Material>()
            .With(m => m.Color, color)
            .With(m => m.Ambient, ambient)
            .With(m => m.Specular, specular)
            .With(m => m.Shininess, shininess)
            .Without(m => m.Pattern)
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
            .Without(m => m.Pattern)
            .Create();
        var y = _fixture.Build<Material>()
            .With(m => m.Color, color)
            .With(m => m.Ambient, ambient)
            .With(m => m.Diffuse, diffuse)
            .With(m => m.Shininess, shininess)
            .Without(m => m.Pattern)
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
            .Without(m => m.Pattern)
            .Create();
        var y = _fixture.Build<Material>()
            .With(m => m.Color, color)
            .With(m => m.Ambient, ambient)
            .With(m => m.Diffuse, diffuse)
            .With(m => m.Specular, specular)
            .Without(m => m.Pattern)
            .Create();

        _mockColorComparer.Setup(comparer => comparer.Equals(x.Color, y.Color))
            .Returns(false)
            .Verifiable();

        Assert.False(_comparer.Equals(x, y));
    }

    [Fact]
    public void PatternsAreDifferent()
    {
        var color = _fixture.Create<Color>();
        var ambient = _fixture.Create<decimal>();
        var diffuse = _fixture.Create<decimal>();
        var specular = _fixture.Create<decimal>();
        var shininess = _fixture.Create<decimal>();

        var x = _fixture.Build<Material>()
            .With(m => m.Color, color)
            .With(m => m.Ambient, ambient)
            .With(m => m.Diffuse, diffuse)
            .With(m => m.Specular, specular)
            .With(m => m.Shininess, shininess)
            .With(m => m.Pattern, _fixture.Create<StripePattern>())
            .Create();
        var y = _fixture.Build<Material>()
            .With(m => m.Color, color)
            .With(m => m.Ambient, ambient)
            .With(m => m.Diffuse, diffuse)
            .With(m => m.Specular, specular)
            .With(m => m.Shininess, shininess)
            .With(m => m.Pattern, _fixture.Create<StripePattern>())
            .Create();

        _mockColorComparer.Setup(comparer => comparer.Equals(x.Color, y.Color))
            .Returns(false)
            .Verifiable();

        Assert.False(_comparer.Equals(x, y));
    }
}
