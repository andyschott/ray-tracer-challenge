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

    public void WritePixel(int x, int y, Color color)
    {
        y = Height - y;
        this[x, y] = color;
    }

    public IEnumerator<Color> GetEnumerator()
    {
        return _pixels.Cast<Color>()
            .GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}