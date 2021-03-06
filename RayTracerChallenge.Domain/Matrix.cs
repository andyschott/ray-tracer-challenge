using System.Text;

namespace RayTracerChallenge.Domain;

public class Matrix
{
    private readonly decimal[,] _data;

    public int Width { get; }
    public int Height { get; }

    public static Matrix Identity(int width = 4, int height = 4)
    {
        if(width != height)
        {
            throw new ArgumentException("width and height must be equal");
        }

        var matrix = new Matrix(width, height);
        for(var index = 0; index < width; ++index)
        {
            matrix[index, index] = 1;
        }

        return matrix;
    }

    public Matrix(int width, int height)
    {
        _data = new decimal[width, height];

        Width = width;
        Height = height;
    }

    public Matrix(decimal[,] data)
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

    public Matrix(int width, int height, params decimal[] data)
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

    public decimal this[int y, int x]
    {
        get => _data[x, y];
        set => _data[x, y] = value;
    }

    public IEnumerable<decimal> GetRow(int y)
    {
        return new RowEnumerator(this, y);
    }

    public IEnumerable<decimal> GetColumn(int x)
    {
        return new ColumnEnumerator(this, x);
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
            var row = GetRow(y);
            for(var x = 0; x < Width; ++x)
            {
                var col = other.GetColumn(x);
                product[y, x] = Multiply(row, col);
            }
        }

        return product;
    }

    private static decimal Multiply(IEnumerable<decimal> rowData, IEnumerable<decimal> columnData)
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

    public Matrix Transpose()
    {
        var matrix = new Matrix(Width, Height);

        for(var y = 0; y < Height; ++y)
        {
            for(var x = 0; x < Width; ++x)
            {
                matrix[x,y] = this[y,x];
            }
        }

        return matrix;
    }

    public decimal Determinant()
    {
        if(Width == 2 && Height == 2)
        {
            return this[0,0] * this[1,1] - 
                this[0,1] * this[1,0];
        }

        var row = GetRow(0);
        return row.Select((value, index) => value * Cofactor(0, index))
            .Sum();
    }

    public Matrix ExtractSubMatrix(int row, int column)
    {
        if(column < 0 || column >= Width)
        {
            throw new ArgumentOutOfRangeException(nameof(column));
        }

        if(row < 0 || row >= Height)
        {
            throw new ArgumentOutOfRangeException(nameof(row));
        }

        var matrix = new Matrix(Width - 1, Height - 1);

        for(var y = 0; y < Height; ++y)
        {
            if(y == row)
            {
                continue;
            }
            
            var destY = y > row ? y - 1 : y;
            for(var x = 0; x < Width; ++x)
            {
                if(x == column)
                {
                    continue;
                }

                var destX = x > column ? x - 1 : x;
                matrix[destY,destX] = this[y,x];
            }
        }

        return matrix;
    }

    public decimal Minor(int row, int column)
    {
        if(column < 0 || column >= Width)
        {
            throw new ArgumentOutOfRangeException(nameof(column));
        }

        if(row < 0 || row >= Height)
        {
            throw new ArgumentOutOfRangeException(nameof(row));
        }

        var subMatrix = ExtractSubMatrix(row, column);
        return subMatrix.Determinant();
    }

    public decimal Cofactor(int row, int column)
    {
        var minor = Minor(row, column);

        // Negate the minor of row + column is odd
        if((row + column) % 2 != 0)
        {
            minor *= -1;
        }

        return minor;
    }

    public bool IsInvertable() => Determinant() != 0;

    public Matrix Invert()
    {
        if(!IsInvertable())
        {
            throw new Exception("Matrix is not invertable");
        }

        var determinant = Determinant();

        var matrix = new Matrix(Width, Height);

        for(var y = 0; y < Height; ++y)
        {
            for(var x = 0; x < Width; ++x)
            {
                var cofactor = Cofactor(y, x);

                // Swapping x and y here effectively transposes the matrix
                matrix[x, y] = cofactor / Determinant();
            }
        }

        return matrix;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();

        for(var y = 0; y < Height; ++y)
        {
            var row = GetRow(y);
            sb.AppendLine(string.Join(", ", row.Select(value => value.ToString("F"))));
        }

        return sb.ToString();
    }

#region Enumerators
    class RowEnumerator : IEnumerable<decimal>
    {
        private readonly Matrix _matrix;
        private readonly int _row;

        public RowEnumerator(Matrix matrix, int row)
        {
            if(row < 0 || row > matrix.Height)
            {
                throw new ArgumentOutOfRangeException(nameof(row));
            }

            _matrix = matrix;
            _row = row;
        }

        public IEnumerator<decimal> GetEnumerator() => new Enumerator(_matrix, _row);

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

        class Enumerator : IEnumerator<decimal>
        {
            public decimal Current => _matrix[_row, _index];

            private readonly Matrix _matrix;
            private readonly int _row;
            private int _index;

            public Enumerator(Matrix matrix, int row)
            {
                _matrix = matrix;
                _row = row;

                Reset();
            }

            object System.Collections.IEnumerator.Current => Current;

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                ++_index;
                return _index < _matrix.Width;
            }

            public void Reset()
            {
                _index = -1;
            }
        }
    }

    class ColumnEnumerator : IEnumerable<decimal>
    {
        private readonly Matrix _matrix;
        private readonly int _column;

        public ColumnEnumerator(Matrix matrix, int column)
        {
            if(column < 0 || column > matrix.Width)
            {
                throw new ArgumentOutOfRangeException(nameof(column));
            }

            _matrix = matrix;
            _column = column;
        }

        public IEnumerator<decimal> GetEnumerator() => new Enumerator(_matrix, _column);

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

        class Enumerator : IEnumerator<decimal>
        {
            public decimal Current => _matrix[_index, _column];

            private readonly Matrix _matrix;
            private readonly int _column;
            private int _index;

            public Enumerator(Matrix matrix, int column)
            {
                _matrix = matrix;
                _column = column;

                Reset();
            }

            object System.Collections.IEnumerator.Current => Current;

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                ++_index;
                return _index < _matrix.Height;
            }

            public void Reset()
            {
                _index = -1;
            }
        }
    }
#endregion
}
