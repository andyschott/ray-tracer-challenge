using RayTracerChallenge.Domain;

namespace RayTracerChallenge.IO.PPM;

public class PPMWriter
{
    private const int MaxLineLength = 70;

    public string Save(Canvas canvas)
    {
        var writer = new StringWriter();
        writer.WriteLine("P3");
        writer.WriteLine($"{canvas.Width} {canvas.Height}");
        writer.WriteLine("255");

        for(var y = 0; y < canvas.Height; ++y)
        {
            var pixels = GetPixels(canvas, y).Select(pixel =>
            {
                var red = Convert(pixel.Red);
                var green = Convert(pixel.Green);
                var blue = Convert(pixel.Blue);

                return (red, green, blue);
            }).Select(pixel => $"{pixel.red} {pixel.green} {pixel.blue}");

            var line = string.Join(' ', pixels);
            var span = line.AsSpan();
            var index = 0;
            while(index < span.Length)
            {
                var length = Math.Min(MaxLineLength, span.Length - index);
                var chunk = span.Slice(index, length);

                if(chunk.Length == MaxLineLength)
                {
                    var endIndex = chunk.LastIndexOf(' ');
                    chunk = chunk.Slice(0, endIndex);
                }
                writer.WriteLine(chunk);

                index += chunk.Length + 1;
            }
        }

        return writer.ToString();
    }

    private static IEnumerable<Color> GetPixels(Canvas canvas, int y)
    {
        for(var x = 0; x < canvas.Width; ++x)
        {
            yield return canvas[x, y];
        }
    }

    private static int Convert(float value)
    {
        value = Math.Min(1.0F, value);
        value = Math.Max(0.0F, value);

        value = value * 255;
        return (int)Math.Round(value, 0);
    }
}
