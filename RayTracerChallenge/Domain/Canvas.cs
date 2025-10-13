using System.Collections;

namespace RayTracerChallenge.Domain;

public class Canvas : IEnumerable<Color>
{
    public int Width { get; }
    public int Height { get; }
    
    private readonly Color[,] _pixels;
    public Color this[int x, int y]
    {
        get => _pixels[x, y];
        set => _pixels[x, y] = value;
    }

    public Canvas(int width, int height)
    {
        Width = width;
        Height = height;
        
        _pixels = new Color[Width, Height];
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                _pixels[x, y] = new Color(0, 0, 0);
            }
        }
    }

    public async Task Render(Func<int, int, Color> renderer)
    {
        var logicalProcessors = Environment.ProcessorCount;
        var pairs = Enumerable.Range(0, Height)
            .SelectMany(y =>
            {
                var something = Enumerable.Range(0, Width);
                return something.Select(x => (x, y)).ToArray();
            }).ToArray();
        var chunks = pairs.Chunk(logicalProcessors);
        foreach (var chunk in chunks)
        {
            var tasks = chunk.Select(pair =>
            {
                return Task.Run(() =>
                {
                    var color = renderer(pair.x, pair.y);
                    return (pair.x, pair.y, color);
                });
            }).ToArray();
            
            var results = await Task.WhenAll(tasks);
            foreach (var (x, y, color) in results)
            {
                this[x, y] = color;
            }
        }
    }

    public IEnumerator<Color> GetEnumerator()
    {
        return _pixels.Cast<Color>()
            .GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}