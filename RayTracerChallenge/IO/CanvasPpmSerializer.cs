using RayTracerChallenge.Domain;

namespace RayTracerChallenge.IO;

public class CanvasPpmSerializer : ICanvasSerializer
{
    private const string Identifier = "P3";
    private const int MaxColorValue = 255;
    private const int MaxLineLength = 70;
    
    public void Serialize(Canvas canvas, TextWriter writer)
    {
        writer.WriteLine(Identifier);
        writer.WriteLine($"{canvas.Width} {canvas.Height}");
        writer.WriteLine($"{MaxColorValue}");

        for(var y = 0; y < canvas.Height; ++y)
        {
            var pixels = GetPixels(canvas, y).Select(pixel =>
            {
                var red = Convert(pixel.Red);
                var green = Convert(pixel.Green);
                var blue = Convert(pixel.Blue);

                return $"{red} {green} {blue}";
            });

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
                    chunk = chunk[..endIndex];
                }
                writer.WriteLine(chunk);

                index += chunk.Length + 1;
            }
        }
    }

    private static IEnumerable<Color> GetPixels(Canvas canvas, int y)
    {
        for(var x = 0; x < canvas.Width; x++)
        {
            yield return canvas[x, y];
        }
    }
    
    private static int Convert(decimal value)
    {
        value = Math.Min(1, value);
        value = Math.Max(0, value);

        value *= MaxColorValue;
        return (int)Math.Round(value, 0);
    }
}