using System;
using System.Linq;

namespace RayTracerChallenge.Domain.Tests;

public class MatrixTests
{
    private readonly IFixture _fixture = new Fixture();
    private readonly MatrixComparer _matrixComparer = new MatrixComparer();
    private readonly TupleComparer _tupleComparer = new TupleComparer();

    [Theory]
    [InlineData(4, 4)]
    [InlineData(3, 3)]
    [InlineData(2, 2)]
    public void ConstructMatrix(int width, int height)
    {
        var data = CreateTestData(width, height);
        var matrix = new Matrix(data);

        for(var y = 0; y < height; ++y)
        {
            for(var x = 0; x < width; ++x)
            {
                Assert.Equal(data[y, x], matrix[y, x]);
            }
        }
    }

    [Fact]
    public void GetRowData()
    {
        var matrix = new Matrix(4, 4, 1, 2, 3, 4, 5, 6, 7, 8, 9, 8, 7, 6, 5, 4, 3, 2);
        var row = _fixture.Create<int>() % matrix.Height;

        var rowData = matrix.GetRow(row);
        for(var x = 0; x < matrix.Width; ++x)
        {
            Assert.Equal(matrix[row, x], rowData.ElementAt(x));
        }
    }

    [Fact]
    public void GetColumnData()
    {
        var matrix = new Matrix(4, 4);
        var column = _fixture.Create<int>() % matrix.Width;

        var columnData = matrix.GetColumn(column);
        for(var y = 0; y < matrix.Width; ++y)
        {
            Assert.Equal(matrix[column, y], columnData.ElementAt(y));
        }
    }
    
    [Fact]
    public void MultiplyMatrices()
    {
        var matrix1 = new Matrix(4, 4, 1, 2, 3, 4, 5, 6, 7, 8, 9, 8, 7, 6, 5, 4, 3, 2);
        var matrix2 = new Matrix(4, 4, -2, 1, 2, 3, 3, 2, 1, -1, 4, 3, 6, 5, 1, 2, 7, 8);

        var product = matrix1.Multiply(matrix2);

        var expectedResult = new Matrix(4, 4, 20, 22, 50, 48, 44, 54, 114, 108, 40, 58, 110, 102, 16, 26, 46, 42);

        Assert.Equal(expectedResult, product, _matrixComparer);
    }

    [Theory]
    [InlineData(4, 4, 2, 4)]
    [InlineData(4, 4, 4, 2)]
    public void MatricesDifferentDimensions(int m1Width, int m1Height, int m2Width, int m2Height)
    {
        var matrix1 = new Matrix(4, 4);
        var matrix2 = new Matrix(2, 4);

        Assert.Throws<ArgumentException>(() => matrix1.Multiply(matrix2));
    }


    [Fact]
    public void MultiplyTuple()
    {
        var matrix = new Matrix(4, 4, 1, 2, 3, 4, 2, 4, 4, 2, 8, 6, 4, 1, 0, 0, 0, 1);
        var tuple = new Tuple(1, 2, 3, 1);

        var product = matrix.Multiply(tuple);

        var expectedResult = new Tuple(18, 24, 33, 1);

        Assert.Equal(expectedResult, product, _tupleComparer);
    }

    [Fact]
    public void MulitplyByTupleWithInvalidMatrix()
    {
        var matrix = new Matrix(3, 4);
        var tuple = _fixture.Create<Tuple>();

        Assert.Throws<InvalidOperationException>(() => matrix.Multiply(tuple));
    }

    [Fact]
    public void MultiplyMatrixByIdentityMatrix()
    {
        var matrix = new Matrix(CreateTestData(4, 4));
        var identityMatrix = Matrix.IdentityMatrix(4, 4);

        var product = matrix.Multiply(identityMatrix);

        Assert.Equal(matrix, product, _matrixComparer);
    }

    [Fact]
    public void MultiplyTupleByIdentityMatrix()
    {
        var tuple = new Tuple
        {
            X = _fixture.Create<float>(),
            Y = _fixture.Create<float>(),
            Z = _fixture.Create<float>(),
            W = _fixture.Create<float>()
        };
        var identityMatrix = Matrix.IdentityMatrix(4, 4);

        var product = identityMatrix.Multiply(tuple);
        
        Assert.Equal(tuple, product, _tupleComparer);
    }

    private float[,] CreateTestData(int width, int height)
    {
        var data = new float[width, height];
        for(var y = 0; y < height; ++y)
        {
            for(var x = 0; x < width; ++x)
            {
                data[y, x] = _fixture.Create<float>();
            }
        }

        return data;
    }
}
