namespace RayTracerChallenge.Domain;

public sealed record Matrix
{
    public int Width { get; }
    public int Height { get; }
    
    private readonly decimal[,] _values;

    public decimal this[int y, int x]
    {
        get => _values[y, x];
        set => _values[y, x] = value;
    }

    public static Matrix Identity = new Matrix(4, 4)
    {
        [0, 0] = 1,
        [0, 1] = 0,
        [0, 2] = 0,
        [0, 3] = 0,
        [1, 0] = 0,
        [1, 1] = 1,
        [1, 2] = 0,
        [1, 3] = 0,
        [2, 0] = 0,
        [2, 1] = 0,
        [2, 2] = 1,
        [2, 3] = 0,
        [3, 0] = 0,
        [3, 1] = 0,
        [3, 2] = 0,
        [3, 3] = 1
    };

    public Matrix(int width, int height)
    {
        Width = width;
        Height = height;
        _values = new decimal[height, width];
    }

    public bool Equals(Matrix? other)
    {
        if (ReferenceEquals(null, other))
        {
            return true;
        }

        if (Width != other.Width ||
            Height != other.Height)
        {
            return false;
        }

        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                var diff = Math.Abs(this[y, x] - other[y, x]);
                if (diff > 0.00001M)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public override int GetHashCode()
    {
        var hashCode = 31;
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                hashCode = hashCode * 43 + this[y, x].GetHashCode();
            }
        }

        return hashCode;
    }

    public Matrix Transpose()
    {
        var result = new Matrix(Width, Height);

        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                result[y, x] = this[x, y];
            }
        }
        
        return result;
    }

    public decimal Determinant()
    {
        if (this is { Width: 2, Height: 2 })
        {
            return this[0, 0] * this[1, 1] -
                   this[0, 1] * this[1, 0];
        }

        var determinant = 0M;
        for (var x = 0; x < Width; x++)
        {
            determinant += this[0, x] * Cofactor(0, x);
        }
        
        return determinant;
    }

    public Matrix Submatrix(int row, int col)
    {
        var result = new Matrix(Width - 1, Height - 1);

        var rowOffset = 0;
        for (var y = 0; y < Height; y++)
        {
            if (y == row)
            {
                rowOffset = 1;
                continue;
            }
            var colOffset = 0;
            
            for (var x = 0; x < Width; x++)
            {
                if (x == col)
                {
                    colOffset = 1;
                    continue;
                }
                
                result[y - rowOffset, x - colOffset] = this[y, x];
            }
        }

        return result;
    }

    public decimal Minor(int row, int col)
    {
        var submatrix = Submatrix(row, col);
        return submatrix.Determinant();
    }

    public decimal Cofactor(int row, int col)
    {
        var minor = Minor(row, col);
        if ((row + col) % 2 == 0)
        {
            return minor;
        }

        return -minor;
    }

    public bool IsInvertible()
    {
        var determinant = Determinant();
        return determinant is not 0;
    }

    public Matrix Inverse()
    {
        if (!IsInvertible())
        {
            throw new InvalidOperationException("Matrix is not invertible.");
        }
        
        var determinant = Determinant();

        var result = new Matrix(Width, Height);
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                var cofactor = Cofactor(y, x);
                result[x, y] = cofactor / determinant;
            }
        }

        return result;
    }
    
    public static Matrix operator *(Matrix left, Matrix right)
    {
        if (left is not { Width: 4, Height: 4 })
        {
            throw new ArgumentException("Only support multiplying 4x4 matrices", nameof(left));
        }
        if (right is not { Width: 4, Height: 4 })
        {
            throw new ArgumentException("Only support multiplying 4x4 matrices", nameof(right));
        }

        var result = new Matrix(4, 4);

        for (var y = 0; y < result.Height; y++)
        {
            for (var x = 0; x < result.Width; x++)
            {
                result[y, x] = left[y, 0] * right[0, x] +
                               left[y, 1] * right[1, x] +
                               left[y, 2] * right[2, x] +
                               left[y, 3] * right[3, x];
            }
        }

        return result;
    }

    public static Tuple operator *(Matrix left, Tuple right)
    {
        if (left is not { Width: 4, Height: 4 })
        {
            throw new ArgumentException("Only support multiplying 4x4 matrices", nameof(left));
        }

        var x = left[0, 0] * right.X +
                left[0, 1] * right.Y +
                left[0, 2] * right.Z +
                left[0, 3] * right.W;
        var y = left[1, 0] * right.X +
                left[1, 1] * right.Y +
                left[1, 2] * right.Z +
                left[1, 3] * right.W;
        var z = left[2, 0] * right.X +
                left[2, 1] * right.Y +
                left[2, 2] * right.Z +
                left[2, 3] * right.W;
        var w = left[3, 0] * right.X +
                left[3, 1] * right.Y +
                left[3, 2] * right.Z +
                left[3, 3] * right.W;

        return new Tuple(x, y, z, w);
    }
};