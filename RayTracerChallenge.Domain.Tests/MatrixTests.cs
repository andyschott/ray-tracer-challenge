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
        var identityMatrix = Matrix.Identity(4, 4);

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

        Assert.Equal(17.0F, determinant);
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
        Assert.Equal(-160F / 532F, inverse[3, 2]);
        Assert.Equal(105, matrix.Cofactor(3, 2));
        Assert.Equal(105F / 532F, inverse[2, 3]);

        var expectedResult = new Matrix(4, 4, 0.21805F, 0.45113F, 0.24060F, -0.04511F,
            -0.80827F, -1.45677F, -0.44361F, 0.52068F,
            -0.07895F, -0.22368F, -0.05263F, 0.19737F,
            -0.52256F, -0.81391F, -0.30075F, 0.30639F);
        Assert.Equal(expectedResult, inverse, _matrixComparer);
    }

    [Theory]
    [MemberData(nameof(InvertTestData))]
    public void InvertTestCases(float[] input, float[] result)
    {
        var matrix = new Matrix(4, 4, input);

        var inverse = matrix.Invert();

        var expectedResult = new Matrix(4, 4, result);
        Assert.Equal(expectedResult, inverse, _matrixComparer);
    }

    public static TheoryData<float[], float[]> InvertTestData => new TheoryData<float[], float[]>
    {
        {
            new[] { 8F, -5F, 9F, 2F, 7F, 5F, 6F, 1F, -6F, 0F, 9F, 6F, -3F, 0F, -9F, -4F },
            new[]
            {
                -0.15385F, -0.15385F, -0.28205F, -0.53846F,
                -0.07692F, 0.12308F, 0.02564F, 0.03077F,
                0.35897F, 0.35897F, 0.43590F, 0.92308F,
                -0.69231F, -0.69231F, -0.76923F, -1.92308F
            }
        },
        {
            new[] { 9F, 3F, 0F, 9F, -5F, -2F, -6F, -3F, -4F, 9F, 6F, 4F, -7F, 6F, 6F, 2F },
            new[]
            {
                -0.04074F, -0.07778F,  0.14444F, -0.22222F,
                -0.07778F,  0.03333F,  0.36667F, -0.33333F,
                -0.02901F, -0.14630F, -0.10926F,  0.12963F,
                 0.17778F,  0.06667F, -0.26667F,  0.33333F
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
