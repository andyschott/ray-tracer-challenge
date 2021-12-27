using System;

namespace RayTracerChallenge.Domain.Tests;

public class MaterialTests
{
    private readonly IFixture _fixture = new Fixture();

    private readonly ColorComparer _colorComparer = new ColorComparer();

    public MaterialTests()
    {
        _fixture.Register(() => new Light(CreatePoint(),
            _fixture.Create<Color>()));
    }

    [Fact]
    public void DefaultValues()
    {
        var material = new Material();

        var black = new Color
        {
            Red = 1,
            Green = 1,
            Blue = 1
        };

        Assert.Equal(black, material.Color, _colorComparer);
        Assert.Equal(0.1M, material.Ambient);
        Assert.Equal(0.9M, material.Diffuse);
        Assert.Equal(0.9M, material.Specular);
        Assert.Equal(200.0M, material.Shininess);
    }

    [Fact]
    public void LightingPointMustBePoint()
    {
        var point = CreateVector();
        var light = _fixture.Create<Light>();
        var eye = CreateVector();
        var normal = CreateVector();

        var material = _fixture.Create<Material>();

        Assert.Throws<ArgumentException>(() => material.Lighting(point, light, eye, normal));
    }

    [Fact]
    public void EyeMustBeVector()
    {
        var point = CreatePoint();
        var light = _fixture.Create<Light>();
        var eye = CreatePoint();
        var normal = CreateVector();

        var material = _fixture.Create<Material>();

        Assert.Throws<ArgumentException>(() => material.Lighting(point, light, eye, normal));
    }

    [Fact]
    public void NormalMustBeVector()
    {
        var point = CreatePoint();
        var light = _fixture.Create<Light>();
        var eye = CreateVector();
        var normal = CreatePoint();

        var material = _fixture.Create<Material>();

        Assert.Throws<ArgumentException>(() => material.Lighting(point, light, eye, normal));
    }

    [Theory]
    [MemberData(nameof(LightingTestData))]
    public void LightingTests(Tuple eye, Tuple normal, Tuple lightPosition,
        Color expectedResult)
    {
        var material = new Material();
        var position = Tuple.CreatePoint(0, 0, 0);
        var light = new Light(lightPosition, new Color
        {
            Red = 1,
            Green = 1,
            Blue = 1
        });

        var result = material.Lighting(position, light, eye, normal);

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

    private Tuple CreatePoint()
    {
        return _fixture.Build<Tuple>()
            .With(tuple => tuple.W, Tuple.Point)
            .Create();
    }

    private Tuple CreateVector()
    {
        return _fixture.Build<Tuple>()
            .With(tuple => tuple.W, Tuple.Vector)
            .Create();
    }
}