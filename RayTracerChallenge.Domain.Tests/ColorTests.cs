namespace RayTracerChallenge.Domain.Tests;

public class ColorTests
{
    private readonly IFixture _fixture = new Fixture();

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
}