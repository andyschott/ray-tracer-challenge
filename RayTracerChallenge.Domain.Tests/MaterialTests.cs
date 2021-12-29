using System;
using RayTracerChallenge.Domain.Tests.Extensions;
using RayTracerChallenge.Domain.Patterns;

namespace RayTracerChallenge.Domain.Tests;

public class MaterialTests
{
    private readonly IFixture _fixture = new Fixture();

    private readonly ColorComparer _colorComparer = new ColorComparer();

    public MaterialTests()
    {
        _fixture.Register(() => new Light(_fixture.CreatePoint(),
            _fixture.Create<Color>()));
        _fixture.Customize<Material>(c => c.Without(m => m.Pattern));
    }

    [Fact]
    public void DefaultValues()
    {
        var material = new Material();

        Assert.Equal(0.1M, material.Ambient);
        Assert.Equal(0.9M, material.Diffuse);
        Assert.Equal(0.9M, material.Specular);
        Assert.Equal(200.0M, material.Shininess);

        Assert.IsType<SolidPattern>(material.Pattern);

        var solidPattern = (SolidPattern)material.Pattern;
        Assert.Equal(Color.White, solidPattern.Color, _colorComparer);
    }

    [Fact]
    public void LightingPointMustBePoint()
    {
        var shape = new TestShape();
        var light = _fixture.Create<Light>();
        var point = _fixture.CreateVector();
        var eye = _fixture.CreateVector();
        var normal = _fixture.CreateVector();

        var material = _fixture.Create<Material>();

        Assert.Throws<ArgumentException>(() => material.Lighting(shape, light, point, eye, normal));
    }

    [Fact]
    public void EyeMustBeVector()
    {
        var shape = new TestShape();
        var light = _fixture.Create<Light>();
        var point = _fixture.CreatePoint();
        var eye = _fixture.CreatePoint();
        var normal = _fixture.CreateVector();

        var material = _fixture.Create<Material>();

        Assert.Throws<ArgumentException>(() => material.Lighting(shape, light, point, eye, normal));
    }

    [Fact]
    public void NormalMustBeVector()
    {
        var shape = new TestShape();
        var light = _fixture.Create<Light>();
        var point = _fixture.CreatePoint();
        var eye = _fixture.CreateVector();
        var normal = _fixture.CreatePoint();

        var material = _fixture.Create<Material>();

        Assert.Throws<ArgumentException>(() => material.Lighting(shape, light, point, eye, normal));
    }

    [Theory]
    [MemberData(nameof(LightingTestData))]
    public void LightingTests(Tuple eye, Tuple normal, Tuple lightPosition,
        Color expectedResult)
    {
        var material = new Material();
        var shape = new TestShape();
        var light = new Light(lightPosition, new Color
        {
            Red = 1,
            Green = 1,
            Blue = 1
        });
        var position = Tuple.CreatePoint(0, 0, 0);

        var result = material.Lighting(shape, light, position, eye, normal);

        Assert.Equal(expectedResult, result, _colorComparer);
    }

    public static TheoryData<Tuple, Tuple, Tuple, Color> LightingTestData => new TheoryData<Tuple, Tuple, Tuple, Color>
    {
        // Eye between light and surface
        {
            Tuple.CreateVector(0, 0, -1),
            Tuple.CreateVector(0, 0, -1),
            Tuple.CreatePoint(0, 0, -10),
            new Color
            {
                Red = 1.9M,
                Green = 1.9M,
                Blue = 1.9M
            }
        },
        // Eye between light and surface, eye offset 45 degrees
        {
            Tuple.CreateVector(0, Convert.ToDecimal(Math.Sqrt(2) / 2), -1 * Convert.ToDecimal(Math.Sqrt(2) / 2)),
            Tuple.CreateVector(0, 0, -1),
            Tuple.CreatePoint(0, 0, -10),
            new Color
            {
                Red = 1,
                Green = 1,
                Blue = 1
            }
        },
        // Eye opposite surface, light offset 45 degrees
        {
            Tuple.CreateVector(0, 0, -1),
            Tuple.CreateVector(0, 0, -1),
            Tuple.CreatePoint(0, 10, -10),
            new Color
            {
                Red = 0.7364M,
                Green = 0.7364M,
                Blue = 0.7364M
            }
        },
        // Eye in the path of the reflection vector
        {
            Tuple.CreateVector(0, -1 * Convert.ToDecimal(Math.Sqrt(2) / 2), -1 * Convert.ToDecimal(Math.Sqrt(2) / 2)),
            Tuple.CreateVector(0, 0, -1),
            Tuple.CreatePoint(0, 10, -10),
            new Color
            {
                Red = 1.6364M,
                Green = 1.6364M,
                Blue = 1.6364M
            }
        },
        // Light behind surface
        {
            Tuple.CreateVector(0, 0, -1),
            Tuple.CreateVector(0, 0, -1),
            Tuple.CreatePoint(0, 0, 10),
            new Color
            {
                Red = 0.1M,
                Green = 0.1M,
                Blue = 0.1M
            }
        },
    };

    [Fact]
    public void LightingWithShadow()
    {
        var material = new Material();
        var shape = new TestShape();
        var position = Tuple.CreatePoint(0, 0, 0);
        var eye = Tuple.CreateVector(0, 0, -1);
        var normal = Tuple.CreateVector(0, 0, -1);

        var light = new Light(Tuple.CreatePoint(0, 0, -10), Color.White);

        var result = material.Lighting(shape, light, position, eye, normal, true);

        var expectedResult = new Color
        {
            Red = 0.1M,
            Green = 0.1M,
            Blue = 0.1M
        };
        Assert.Equal(expectedResult, result, _colorComparer);
    }

    [Theory]
    [InlineData(0.9, 0, 0, true)]
    [InlineData(1.1, 0, 0, false)]
    public void LightingWithStripePatternApplied(decimal x, decimal y, decimal z, bool expectFirstColor)
    {
        var pattern = new StripePattern(Color.White, Color.Black);
        var material = new Material
        {
            Pattern = pattern,
            Ambient = 1,
            Diffuse = 0,
            Specular = 0
        };
        var shape = new TestShape();
        var eye = Tuple.CreateVector(0, 0, -1);
        var normal = Tuple.CreateVector(0, 0, -1);
        var light = new Light(Tuple.CreatePoint(0, 0, -10), Color.White);

        var result = material.Lighting(shape, light, Tuple.CreatePoint(x, y, z), eye, normal);

        Assert.Equal(expectFirstColor ? pattern.First : pattern.Second, result, _colorComparer);
    }
}
