namespace RayTracerChallenge.Domain;

public class Matrix
{
    private readonly float[,] _data;

    public int Width { get; }
    public int Height { get; }

    public Matrix(int width, int height)
    {
        _data = new float[width, height];

        Width = width;
        Height = height;
    }

    public Matrix(float[,] data)
        : this(data.GetLength(0), data.GetLength(1))
    {
        for(var y = 0; y < Height; ++y)
        {
            for(var x = 0; x < Width; ++x)
            {
                this[y, x] = data[y, x];
            }
        }
    }

    public float this[int y, int x]
    {
        get => _data[x, y];
        set => _data[x, y] = value;
    }
}
