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

    public Matrix(int width, int height, params float[] data)
        : this(width, height)
    {
        var index = 0;
        for(var y = 0; y < Height; ++y)
        {
            for(var x = 0; x < Width; ++x)
            {
                this[y, x] = data[index];
                ++index;
            }
        }
    }

    public float this[int y, int x]
    {
        get => _data[x, y];
        set => _data[x, y] = value;
    }

    public IEnumerable<float> GetRow(int y)
    {
        var data = new float[Width];
        for(var x = 0; x < Width; ++x)
        {
            data[x] = this[y, x];
        }

        return data;
    }

    public IEnumerable<float> GetColumn(int x)
    {
        var data = new float[Height];
        for(var y = 0; y < Height; ++y)
        {
            data[y] = this[y, x];
        }

        return data;
    }

    public Matrix Multiply(Matrix other)
    {
        if(Width != other.Width)
        {
            throw new ArgumentException("Both matrices must have the same width", nameof(other));
        }
        if(Height != other.Height)
        {
            throw new ArgumentException("Both matrices must have the same height", nameof(other));
        }

        var product = new Matrix(Width, Height);
        for(var y = 0; y < Height; ++y)
        {
            for(var x = 0; x < Width; ++x)
            {
                product[y, x] = Multiply(other, y, x);
            }
        }

        return product;
    }

    private float Multiply(Matrix other, int y, int x)
    {
        var rowData = GetRow(y);
        var columnData = other.GetColumn(x);

        return Multiply(rowData, columnData);
    }

    private static float Multiply(IEnumerable<float> rowData, IEnumerable<float> columnData)
    {
        return rowData.Zip(columnData).Sum(item => item.First * item.Second);
    }

    public static Matrix operator *(Matrix m1, Matrix m2) => m1.Multiply(m2);

    public Tuple Multiply(Tuple tuple)
    {
        if(Width != 4)
        {
            throw new InvalidOperationException("Can only multiply by a tuple when the matrix has a width of 4");
        }

        var tupleData = new[] { tuple.X, tuple.Y, tuple.Z, tuple.W };

        return new Tuple
        {
            X = Multiply(GetRow(0), tupleData),
            Y = Multiply(GetRow(1), tupleData),
            Z = Multiply(GetRow(2), tupleData),
            W = Multiply(GetRow(3), tupleData)
        };
    }

    public static Tuple operator *(Matrix m, Tuple t) => m.Multiply(t);
}
