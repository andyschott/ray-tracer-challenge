using RayTracerChallenge.Domain;
using RayTracerChallenge.IO;

namespace RayTracerChallenge.Tests.Domain;

public class CanvasPpmSerializerTests
{
    private readonly CanvasPpmSerializer _serializer = new();

    [Fact]
    public void CanvasSerializedSuccessfully()
    {
        var c = new Canvas(5, 3)
        {
            [0, 0] = new Color(1.5M, 0, 0),
            [2, 1] = new Color(0, 0.5M, 0),
            [4, 2] = new Color(-0.5M, 0, 1)
        };

        var ppm = _serializer.Serialize(c);
        
        var lines = ppm.Split(System.Environment.NewLine);
        Assert.Equal(7, lines.Length);
        
        Assert.Equal("P3", lines[0]);
        Assert.Equal("5 3", lines[1]);
        Assert.Equal("255", lines[2]);
        
        Assert.Equal("255 0 0 0 0 0 0 0 0 0 0 0 0 0 0", lines[3]);
        Assert.Equal("0 0 0 0 0 0 0 128 0 0 0 0 0 0 0", lines[4]);
        Assert.Equal("0 0 0 0 0 0 0 0 0 0 0 0 0 0 255", lines[5]);
    }

    [Fact]
    public void VerifyMaxLineLength()
    {
        var c = new Canvas(10, 2);
        for (var x = 0; x < c.Width; x++)
        {
            for (var y = 0; y < c.Height; y++)
            {
                c[x, y] = new Color(1, 0.8M, 0.6M);
            }
        }
        
        var ppm = _serializer.Serialize(c);

        var lines = ppm.Split(System.Environment.NewLine);
        Assert.Equal(8, lines.Length);
        
        Assert.Equal("255 204 153 255 204 153 255 204 153 255 204 153 255 204 153 255 204", lines[3]);
        Assert.Equal("153 255 204 153 255 204 153 255 204 153 255 204 153", lines[4]);
        Assert.Equal("255 204 153 255 204 153 255 204 153 255 204 153 255 204 153 255 204", lines[5]);
        Assert.Equal("153 255 204 153 255 204 153 255 204 153 255 204 153", lines[6]);
    }

    [Fact]
    public void FileEndsWithNewLine()
    {
        var c = new Canvas(5, 3);
        var ppm = _serializer.Serialize(c);

        Assert.EndsWith(System.Environment.NewLine, ppm);
    }
}