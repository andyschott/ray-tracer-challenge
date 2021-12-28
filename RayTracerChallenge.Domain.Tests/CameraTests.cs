using Math = System.Math;

namespace RayTracerChallenge.Domain.Tests;

public class CameraTests
{
    private readonly TransformationFactory _factory = new TransformationFactory();

    private readonly ColorComparer _colorComparer = new ColorComparer();
    private readonly MatrixComparer _matrixComparer = new MatrixComparer();
    private readonly RayComparer _rayComparer = new RayComparer();

    [Fact]
    public void ConstructCamera()
    {
        var camera = new Camera(160, 120, Math.PI / 2);

        Assert.Equal(160, camera.Width);
        Assert.Equal(120, camera.Height);
        Assert.Equal(Math.PI / 2, camera.FieldOfView);
        Assert.Equal(Matrix.Identity(), camera.Transform, _matrixComparer);
    }

    [Theory]
    [InlineData(200, 125, 0.01)]
    [InlineData(125, 200, 0.01)]
    public void CorrectlyComputesPixelSize(int width, int height, decimal expectedPixelSize)
    {
        var camera = new Camera(width, height, Math.PI / 2);

        Assert.Equal(expectedPixelSize, camera.PixelSize);
    }

    [Fact]
    public void RayThroughCenterOfCanvas()
    {
        var camera = new Camera(201, 101, Math.PI / 2);

        var result = camera.RayForPixel(100, 50);

        var expectedResult = new Ray(Tuple.CreatePoint(0, 0, 0),
            Tuple.CreateVector(0, 0, -1));
        Assert.Equal(expectedResult, result, _rayComparer);
    }

    [Fact]
    public void RayThroughCornerOfCanvas()
    {
        var camera = new Camera(201, 101, Math.PI / 2);

        var result = camera.RayForPixel(0, 0);

        var expectedResult = new Ray(Tuple.CreatePoint(0, 0, 0),
            Tuple.CreateVector(0.66519M, 0.33259M, -0.66851M));
        Assert.Equal(expectedResult, result, _rayComparer);
    }

    [Fact]
    public void RayWhenCameraIsTransformed()
    {
        var rotation = _factory.RotationAroundYAxis(Math.PI / 4);
        var translation = _factory.Translation(0, -2, 5);

        var camera = new Camera(201, 101, Math.PI / 2,
            rotation * translation);

        var result = camera.RayForPixel(100, 50);

        var expectedResult = new Ray(Tuple.CreatePoint(0, 2, -5),
            Tuple.CreateVector(0.70710M, 0, -0.70710M));
        Assert.Equal(expectedResult, result, _rayComparer);
    }

    [Fact]
    public void RenderWorld()
    {
        var world = World.Default();

        var from = Tuple.CreatePoint(0, 0, -5);
        var to = Tuple.CreatePoint(0, 0, 0);
        var up = Tuple.CreateVector(0, 1, 0);

        var camera = new Camera(11, 11, Math.PI / 2,
            _factory.TransformView(from, to, up));

        var canvas = camera.Render(world);

        var expectedPixel = new Color
        {
            Red = 0.38066M,
            Green = 0.47583M,
            Blue = 0.2855M
        };
        Assert.Equal(expectedPixel, canvas[5, 5], _colorComparer);
    }
}
