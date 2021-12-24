namespace RayTracerChallenge.Domain.Tests;

public class ColorTests
{
    private readonly IFixture _fixture = new Fixture();
    private readonly ColorComparer _comparer = new ColorComparer();

    [Fact]
    public void ColorValues()
    {
        var red = _fixture.Create<float>();
        var green = _fixture.Create<float>();
        var blue = _fixture.Create<float>();

        var color = _fixture.Build<Color>()
            .With(color => color.Red, red)
            .With(color => color.Green, green)
            .With(color => color.Blue, blue)
            .Create();

        Assert.Equal(red, color.Red);
        Assert.Equal(green, color.Green);
        Assert.Equal(blue, color.Blue);
    }    

    [Fact]
    public void AddColors()
    {
        var color1 = _fixture.Create<Color>();
        var color2 = _fixture.Create<Color>();

        var sum = color1.Add(color2);

        var expectedSum = new Color
        {
            Red = color1.Red + color2.Red,
            Green = color1.Green + color2.Green,
            Blue = color1.Blue + color2.Blue
        };

        Assert.Equal(expectedSum, sum, _comparer);
    }

    [Fact]
    public void SubtractColors()
    {
        var color1 = _fixture.Create<Color>();
        var color2 = _fixture.Create<Color>();

        var difference = color1.Subtract(color2);

        var expectedDifference = new Color
        {
            Red = color1.Red - color2.Red,
            Green = color1.Green - color2.Green,
            Blue = color1.Blue - color2.Blue
        };

        Assert.Equal(expectedDifference, difference, _comparer);
    }

    [Fact]
    public void MultiplyByScalar()
    {
        var color = _fixture.Create<Color>();
        var scalar = _fixture.Create<float>();

        var result = color.Multiply(scalar);

        var expectedColor = new Color
        {
            Red = color.Red * scalar,
            Green = color.Green * scalar,
            Blue = color.Blue * scalar
        };

        Assert.Equal(expectedColor, result, _comparer);
    }

    [Fact]
    public void MultiplyColors()
    {
        var color1 = _fixture.Create<Color>();
        var color2 = _fixture.Create<Color>();

        var product = color1.Multiply(color2);

        var expectedProduct = new Color
        {
            Red = color1.Red * color2.Red,
            Green = color1.Green * color2.Green,
            Blue = color1.Blue * color2.Blue
        };

        Assert.Equal(expectedProduct, product, _comparer);
    }
}