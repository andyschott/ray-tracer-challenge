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
        var matrix1 = new Matrix(m1Width, m1Height);
        var matrix2 = new Matrix(m2Width, m2Height);

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
        var identityMatrix = Matrix.Identity(4, 4);

        var product = matrix.Multiply(identityMatrix);

        Assert.Equal(matrix, product, _matrixComparer);
    }

    [Fact]
    public void MultiplyTupleByIdentityMatrix()
    {
        var tuple = new Tuple
        {
            X = _fixture.Create<decimal>(),
            Y = _fixture.Create<decimal>(),
            Z = _fixture.Create<decimal>(),
            W = _fixture.Create<decimal>()
        };
        var identityMatrix = Matrix.Identity(4, 4);

        var product = identityMatrix.Multiply(tuple);
        
        Assert.Equal(tuple, product, _tupleComparer);
    }

    [Fact]
    public void TransposeMatrix()
    {
        var matrix = new Matrix(CreateTestData(4, 4));

        var result = matrix.Transpose();

        for(var y = 0; y < matrix.Height; ++y)
        {
            for(var x = 0; x < matrix.Width; ++x)
            {
                Assert.Equal(matrix[x,y], result[y,x]);
            }
        }
    }

    [Fact]
    public void TransposeIdentityMatrix()
    {
        var identityMatrix = Matrix.Identity(4, 4);

        var result = identityMatrix.Transpose();

        Assert.Equal(identityMatrix, result, _matrixComparer);
    }

    [Fact]
    public void CalculateDeterminantOf2x2Matrix()
    {
        var matrix = new Matrix(2, 2, 1, 5, -3, 2);

        var determinant = matrix.Determinant();

        Assert.Equal(17, determinant);
    }

    [Fact]
    public void Extract3x3Matrix()
    {
        var matrix = new Matrix(3, 3, 1, 5, 0, -3, 2, 7, 0, 6, -3);

        var subMatrix = matrix.ExtractSubMatrix(0, 2);

        var expectedResult = new Matrix(2, 2, -3, 2, 0, 6);

        Assert.Equal(expectedResult, subMatrix, _matrixComparer);
    }

    [Fact]
    public void Extract4x4Matrix()
    {
        var matrix = new Matrix(4, 4, -6, 1, 1, 6, -8, 5, 8, 6, -1, 0, 8, 2, -7, 1, -1, 1);

        var subMatrix = matrix.ExtractSubMatrix(2, 1);

        var expectedResult = new Matrix(3, 3, -6, 1, 6, -8, 8, 6, -7, -1, 1);

        Assert.Equal(expectedResult, subMatrix, _matrixComparer);
    }

    [Fact]
    public void Minor()
    {
        var matrix = new Matrix(3, 3, 3, 5, 0, 2, -1, -7, 6, -1, 5);

        var minor = matrix.Minor(1, 0);

        var subMatrix = matrix.ExtractSubMatrix(1, 0);
        var determinant = subMatrix.Determinant();

        Assert.Equal(determinant, minor);
    }

    [Fact]
    public void Cofactor()
    {
        var matrix = new Matrix(3, 3, 3, 5, 0, 2, -1, -7, 6, -1, 5);

        var cofactor = matrix.Cofactor(0, 0);
        var minor = matrix.Minor(0, 0);
        Assert.Equal(minor, cofactor);

        cofactor = matrix.Cofactor(1, 0);
        minor = matrix.Minor(1, 0);
        Assert.Equal(minor * -1, cofactor);
    }

    [Fact]
    public void CalculateDeterminantOf3x3Matrix()
    {
        var matrix = new Matrix(3, 3, 1, 2, 6, -5, 8, -4, 2, 6, 4);

        var cofactor = matrix.Cofactor(0, 0);
        Assert.Equal(56, cofactor);

        cofactor = matrix.Cofactor(0, 1);
        Assert.Equal(12, cofactor);

        cofactor = matrix.Cofactor(0, 2);
        Assert.Equal(-46, cofactor);

        var determinant = matrix.Determinant();
        Assert.Equal(-196, determinant);
    }

    [Fact]
    public void CalculateDeterminantOf4x4Matrix()
    {
        var matrix = new Matrix(4, 4, -2, -8, 3, 5, -3, 1, 7, 3, 1, 2, -9, 6, -6, 7, 7, -9);

        var cofactor = matrix.Cofactor(0, 0);
        Assert.Equal(690, cofactor);

        cofactor = matrix.Cofactor(0, 1);
        Assert.Equal(447, cofactor);

        cofactor = matrix.Cofactor(0, 2);
        Assert.Equal(210, cofactor);

        cofactor = matrix.Cofactor(0, 3);
        Assert.Equal(51, cofactor);

        var determinant = matrix.Determinant();
        Assert.Equal(-4071, determinant);
    }

    [Fact]
    public void MatrixIsInvertable()
    {
        var matrix = new Matrix(4, 4, 6, 4, 4, 4, 5, 5, 7, 6, 4, -9, 3, -7, 9, 1, 7, -6);
        Assert.True(matrix.IsInvertable());
    }

    [Fact]
    public void MatrixIsNotInvertable()
    {
        var matrix = new Matrix(4, 4, -4, 2, -2, -3, 9, 6, 2, 6, 0, -5, 1, -5, 0, 0, 0, 0);
        Assert.False(matrix.IsInvertable());
    }

    [Fact]
    public void Invert()
    {
        var matrix = new Matrix(4, 4, -5, 2, 6, -8, 1, -5, 1, 8, 7, 7, -6, -7, 1, -3, 7, 4);

        var inverse = matrix.Invert();

        Assert.Equal(532, matrix.Determinant());
        Assert.Equal(-160, matrix.Cofactor(2, 3));
        Assert.Equal(-160M / 532M, inverse[3, 2]);
        Assert.Equal(105, matrix.Cofactor(3, 2));
        Assert.Equal(105M / 532M, inverse[2, 3]);

        var expectedResult = new Matrix(4, 4, 0.21805M, 0.45113M, 0.24060M, -0.04511M,
            -0.80827M, -1.45677M, -0.44361M, 0.52068M,
            -0.07895M, -0.22368M, -0.05263M, 0.19737M,
            -0.52256M, -0.81391M, -0.30075M, 0.30639M);
        Assert.Equal(expectedResult, inverse, _matrixComparer);
    }

    [Theory]
    [MemberData(nameof(InvertTestData))]
    public void InvertTestCases(decimal[] input, decimal[] result)
    {
        var matrix = new Matrix(4, 4, input);

        var inverse = matrix.Invert();

        var expectedResult = new Matrix(4, 4, result);
        Assert.Equal(expectedResult, inverse, _matrixComparer);
    }

    public static TheoryData<decimal[], decimal[]> InvertTestData => new TheoryData<decimal[], decimal[]>
    {
        {
            new[] { 8M, -5M, 9M, 2M, 7M, 5M, 6M, 1M, -6M, 0M, 9M, 6M, -3M, 0M, -9M, -4M },
            new[]
            {
                -0.15385M, -0.15385M, -0.28205M, -0.53846M,
                -0.07692M, 0.12308M, 0.02564M, 0.03077M,
                0.35897M, 0.35897M, 0.43590M, 0.92308M,
                -0.69231M, -0.69231M, -0.76923M, -1.92308M
            }
        },
        {
            new[] { 9M, 3M, 0M, 9M, -5M, -2M, -6M, -3M, -4M, 9M, 6M, 4M, -7M, 6M, 6M, 2M },
            new[]
            {
                -0.04074M, -0.07778M,  0.14444M, -0.22222M,
                -0.07778M,  0.03333M,  0.36667M, -0.33333M,
                -0.02901M, -0.14630M, -0.10926M,  0.12963M,
                 0.17778M,  0.06667M, -0.26667M,  0.33333M
            }
        }
    };

    [Fact]
    public void MultiplyProductByInverse()
    {
        var matrix1 = new Matrix(4, 4, 3, -9, 7, 3, 3, -8, 2, -9, -4, 4, 4, 1, -6, 5, -1, 1);
        var matrix2 = new Matrix(4, 4, 8, 2, 2, 2, 3, -1, 7, 0, 7, 0, 5, 4, 6, -2, 0, 5);

        var product = matrix1.Multiply(matrix2);
        var inverse2 = matrix2.Invert();
        var result = product.Multiply(inverse2);

        Assert.Equal(matrix1, result, _matrixComparer);
    }

    [Fact]
    public void CannotInvertMatrix()
    {
        var matrix = new Matrix(4, 4, -4, 2, -2, -3, 9, 6, 2, 6, 0, -5, 1, -5, 0, 0, 0, 0);
        Assert.Throws<Exception>(() => matrix.Invert());
    }

    private decimal[,] CreateTestData(int width, int height)
    {
        var data = new decimal[width, height];
        for(var y = 0; y < height; ++y)
        {
            for(var x = 0; x < width; ++x)
            {
                data[y, x] = _fixture.Create<decimal>();
            }
        }

        return data;
    }
}
