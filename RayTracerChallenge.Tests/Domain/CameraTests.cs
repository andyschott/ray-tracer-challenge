using RayTracerChallenge.Domain;
using RayTracerChallenge.Extensions;
using Tuple = RayTracerChallenge.Domain.Tuple;

namespace RayTracerChallenge.Tests.Domain;

public class CameraTests
{
    [Fact]
    public void ConstructCamera()
    {
        var c = new Camera(160, 120, Math.PI / 2);
        
        Assert.Equal(160, c.HorizontalSize);
        Assert.Equal(120, c.VerticalSize);
        Assert.Equal(Math.PI / 2, c.FieldOfView, 4);
        Assert.Equal(Matrix.Identity, c.Transform);
    }

    [Theory]
    [InlineData(200, 125)]
    [InlineData(125, 200)]
    public void PixelSizeIsCorrect(int horizontalSize,
        int verticalSize)
    {
        var c = new Camera(horizontalSize, verticalSize, 
            Math.PI / 2);
        Assert.Equal(0.01, c.PixelSize, 4);
    }

    [Fact]
    public void RayThroughCenterOfCanvas()
    {
        var c = new Camera(201, 101, Math.PI / 2);

        var result = c.RayForPixel(100, 50);
        var expectedResult = new Ray(Tuple.CreatePoint(0, 0, 0),
            Tuple.CreateVector(0, 0, -1));
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void RayThroughCornerOfCanvas()
    {
        var c = new Camera(201, 101, Math.PI / 2);

        var result = c.RayForPixel(0, 0);
        var expectedResult = new Ray(Tuple.CreatePoint(0, 0, 0),
            Tuple.CreateVector(0.66519, 0.33259, -0.66851));
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void RayWhenCameraIsTransformed()
    {
        var c = new Camera(201, 101, Math.PI / 2,
            Matrix.Identity.Translate(0, -2, 5).RotateY(Math.PI / 4));

        var result = c.RayForPixel(100, 50);
        var expectedResult = new Ray(Tuple.CreatePoint(0, 2, -5),
            Tuple.CreateVector(Math.Sqrt(2)/2, 0, -Math.Sqrt(2)/2));
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public async Task RenderWorldWithCamera()
    {
        var w = World.DefaultWorld();

        var from = Tuple.CreatePoint(0, 0, -5);
        var to = Tuple.CreatePoint(0, 0, 0);
        var up = Tuple.CreateVector(0, 1, 0);
        var c = new Camera(11, 11, Math.PI / 2,
            TransformationFactory.View(from, to, up));
        
        var image = await c.Render(w);
        var expectedColor = new Color(0.38066, 0.47583, 0.2855);
        
        Assert.Equal(image[5, 5], expectedColor);
    }
}