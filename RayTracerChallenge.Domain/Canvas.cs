namespace RayTracerChallenge.Domain;

public class Canvas : IEnumerable<Color>
{
    public int Width { get; }
    public int Height { get; }

    private Color[,] _pixels;

    public Canvas(int width, int height)
    {
        Width = width;
        Height = height;

        _pixels = new Color[Width,Height];
        for(var y = 0; y < Height; ++y)
        {
            for(var x = 0; x < Width; ++x)
            {
                _pixels[x,y] = new Color
                {
                    Red = 0,
                    Green = 0,
                    Blue = 0
                };
            }
        }
    }

    public void WritePixel(int x, int y, Color color)
    {
        if(x < 0 || x >= Width)
        {
            throw new ArgumentOutOfRangeException($"{nameof(x)} is out of bounds", nameof(x));
        }
        if(y < 0 || y >= Height)
        {
            throw new ArgumentOutOfRangeException($"{nameof(y)} is out of bounds", nameof(y));
        }

        _pixels[x, y] = color;
    }

    public Color GetPixel(int x, int y)
    {
        if(x < 0 || x >= Width)
        {
            throw new ArgumentOutOfRangeException($"{nameof(x)} is out of bounds", nameof(x));
        }
        if(y < 0 || y >= Height)
        {
            throw new ArgumentOutOfRangeException($"{nameof(y)} is out of bounds", nameof(y));
        }

        return _pixels[x, y];
    }

    public Color this[int x, int y]
    {
        get => GetPixel(x, y);
        set => WritePixel(x, y, value);
    }

    public IEnumerator<Color> GetEnumerator()
    {
        return _pixels.Cast<Color>().GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return _pixels.GetEnumerator();
    }
}
