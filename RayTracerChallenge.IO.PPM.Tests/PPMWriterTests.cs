using System;
using System.Linq;

namespace RayTracerChallenge.IO.PPM.Tests;

public class PPMWriterTests
{
    private readonly IFixture _fixture = new Fixture();
    private readonly PPMWriter _writer = new PPMWriter();

    [Fact]
    public void PPMHeaderIsWrittenCorrectly()
    {
        var canvas = new Canvas(5, 3);

        var ppm = _writer.Save(canvas);

        var header = ppm.Split(Environment.NewLine)
            .Take(3)
            .ToArray();
        
        Assert.Equal("P3", header[0]);
        Assert.Equal($"{canvas.Width} {canvas.Height}", header[1]);
        Assert.Equal("255", header[2]);
    }

    [Fact]
    public void PPMColorDataIsWrittenCorrectly()
    {
        var canvas = new Canvas(5, 3);
        var c1 = new Color
        {
            Red = 1.5F,
            Green = 0.0F,
            Blue = 0.0F
        };
        var c2 = new Color
        {
            Red = 0.0F,
            Green = 0.5F,
            Blue = 0.0F
        };
        var c3 = new Color
        {
            Red = -0.5F,
            Green = 0.0F,
            Blue = 1.0F
        };

        canvas.WritePixel(0, 0, c1);
        canvas.WritePixel(2, 1, c2);
        canvas.WritePixel(4, 2, c3);

        var ppm = _writer.Save(canvas);

        var colorData = ppm.Split(Environment.NewLine)
            .Skip(3)
            .ToArray();

        var line1 = "255 0 0 0 0 0 0 0 0 0 0 0 0 0 0";
        Assert.Equal(line1, colorData[0]);

        var line2 = "0 0 0 0 0 0 0 128 0 0 0 0 0 0 0";
        Assert.Equal(line2, colorData[1]);

        var line3 = "0 0 0 0 0 0 0 0 0 0 0 0 0 0 255";
        Assert.Equal(line3, colorData[2]);
    }

    [Fact]
    public void SplitLongLines()
    {
        var canvas = new Canvas(10, 2);
        for(var y = 0; y < canvas.Height; ++y)
        {
            for(var x = 0; x < canvas.Width; ++x)
            {
                canvas[x, y] = new Color
                {
                    Red = 1.0F,
                    Green = 0.8F,
                    Blue = 0.6F
                };
            }
        }

        var ppm = _writer.Save(canvas);

        var colorData = ppm.Split(Environment.NewLine)
            .Skip(3)
            .ToArray();

        Assert.Equal(5, colorData.Length);

        var line1 = "255 204 153 255 204 153 255 204 153 255 204 153 255 204 153 255 204";
        Assert.Equal(line1, colorData[0]);

        var line2 = "153 255 204 153 255 204 153 255 204 153 255 204 153";
        Assert.Equal(line2, colorData[1]);

        var line3 = "255 204 153 255 204 153 255 204 153 255 204 153 255 204 153 255 204";
        Assert.Equal(line3, colorData[2]);

        var line4 = "153 255 204 153 255 204 153 255 204 153 255 204 153";
        Assert.Equal(line4, colorData[3]);
    }

    [Fact]
    public void FileIsTerminatedWithNewLine()
    {
        var canvas = new Canvas(5, 3);
        var ppm = _writer.Save(canvas);

        Assert.True(ppm.EndsWith(Environment.NewLine));
    }
}