using RayTracerChallenge.Domain;
using Tuple = RayTracerChallenge.Domain.Tuple;

namespace RayTracerChallenge.Tests.Domain;

public class MatrixTests
{
    [Fact]
    public void MatrixConstructedSuccessfully()
    {
        var m = new Matrix(4, 4)
        {
            [0, 0] = 1,
            [0, 1] = 2,
            [0, 2] = 3,
            [0, 3] = 4,
            [1, 0] = 5.5,
            [1, 1] = 6.5,
            [1, 2] = 7.5,
            [1, 3] = 8.5,
            [2, 0] = 9,
            [2, 1] = 10,
            [2, 2] = 11,
            [2, 3] = 12,
            [3, 0] = 13.5,
            [3, 1] = 14.5,
            [3, 2] = 15.5,
            [3, 3] = 16.5
        };

        Assert.Equal(1, m[0, 0]);
        Assert.Equal(4, m[0, 3]);
        Assert.Equal(5.5, m[1, 0]);
        Assert.Equal(7.5, m[1, 2]);
        Assert.Equal(11, m[2, 2]);
        Assert.Equal(13.5, m[3, 0]);
        Assert.Equal(15.5, m[3, 2]);
    }

    [Fact]
    public void Construct2X2Matrix()
    {
        var m = new Matrix(2, 2)
        {
            [0, 0] = -3,
            [0, 1] = 5,
            [1, 0] = 1,
            [1, 1] = -2,
        };
        
        Assert.Equal(-3, m[0, 0]);
        Assert.Equal(5, m[0, 1]);
        Assert.Equal(1, m[1, 0]);
        Assert.Equal(-2, m[1, 1]);
    }

    [Fact]
    public void Construct3X3Matrix()
    {
        var m = new Matrix(3, 3)
        {
            [0, 0] = -3,
            [0, 1] = 5,
            [0, 2] = 0,
            [1, 0] = 1,
            [1, 1] = -2,
            [1, 2] = -7,
            [2, 0] = 0,
            [2, 1] = 1,
            [2, 2] = 1,
        };
        
        Assert.Equal(-3, m[0, 0]);
        Assert.Equal(-2, m[1, 1]);
        Assert.Equal(1, m[2, 2]);
    }

    [Fact]
    public void MatrixEquality_Same()
    {
        var m1 = new Matrix(4, 4)
        {
            [0, 0] = 1,
            [0, 1] = 2,
            [0, 2] = 3,
            [0, 3] = 4,
            [1, 0] = 5,
            [1, 1] = 6,
            [1, 2] = 7,
            [1, 3] = 8,
            [2, 0] = 9,
            [2, 1] = 8,
            [2, 2] = 7,
            [2, 3] = 6,
            [3, 0] = 5,
            [3, 1] = 4,
            [3, 2] = 3,
            [3, 3] = 2,
        };
        var m2 = new Matrix(4, 4)
        {
            [0, 0] = 1,
            [0, 1] = 2,
            [0, 2] = 3,
            [0, 3] = 4,
            [1, 0] = 5,
            [1, 1] = 6,
            [1, 2] = 7,
            [1, 3] = 8,
            [2, 0] = 9,
            [2, 1] = 8,
            [2, 2] = 7,
            [2, 3] = 6,
            [3, 0] = 5,
            [3, 1] = 4,
            [3, 2] = 3,
            [3, 3] = 2,
        };
        
        Assert.Equal(m1, m2);
    }

    [Fact]
    public void MatrixEquality_Different()
    {
        var m1 = new Matrix(4, 4)
        {
            [0, 0] = 1,
            [0, 1] = 2,
            [0, 2] = 3,
            [0, 3] = 4,
            [1, 0] = 5,
            [1, 1] = 6,
            [1, 2] = 7,
            [1, 3] = 8,
            [2, 0] = 9,
            [2, 1] = 8,
            [2, 2] = 7,
            [2, 3] = 6,
            [3, 0] = 5,
            [3, 1] = 4,
            [3, 2] = 3,
            [3, 3] = 2,
        };
        var m2 = new Matrix(4, 4)
        {
            [0, 0] = 2,
            [0, 1] = 3,
            [0, 2] = 4,
            [0, 3] = 5,
            [1, 0] = 6,
            [1, 1] = 7,
            [1, 2] = 8,
            [1, 3] = 9,
            [2, 0] = 8,
            [2, 1] = 7,
            [2, 2] = 6,
            [2, 3] = 5,
            [3, 0] = 4,
            [3, 1] = 3,
            [3, 2] = 2,
            [3, 3] = 1,
        };
        
        Assert.NotEqual(m1, m2);
    }

    [Fact]
    public void MultiplyMatrices()
    {
        var a = new Matrix(4, 4)
        {
            [0, 0] = 1,
            [0, 1] = 2,
            [0, 2] = 3,
            [0, 3] = 4,
            [1, 0] = 5,
            [1, 1] = 6,
            [1, 2] = 7,
            [1, 3] = 8,
            [2, 0] = 9,
            [2, 1] = 8,
            [2, 2] = 7,
            [2, 3] = 6,
            [3, 0] = 5,
            [3, 1] = 4,
            [3, 2] = 3,
            [3, 3] = 2,
        };
        var b = new Matrix(4, 4)
        {
            [0, 0] = -2,
            [0, 1] = 1,
            [0, 2] = 2,
            [0, 3] = 3,
            [1, 0] = 3,
            [1, 1] = 2,
            [1, 2] = 1,
            [1, 3] = -1,
            [2, 0] = 4,
            [2, 1] = 3,
            [2, 2] = 6,
            [2, 3] = 5,
            [3, 0] = 1,
            [3, 1] = 2,
            [3, 2] = 7,
            [3, 3] = 8,
        };

        var product = a * b;
        var expectedResult = new Matrix(4, 4)
        {
            [0, 0] = 20,
            [0, 1] = 22,
            [0, 2] = 50,
            [0, 3] = 48,
            [1, 0] = 44,
            [1, 1] = 54,
            [1, 2] = 114,
            [1, 3] = 108,
            [2, 0] = 40,
            [2, 1] = 58,
            [2, 2] = 110,
            [2, 3] = 102,
            [3, 0] = 16,
            [3, 1] = 26,
            [3, 2] = 46,
            [3, 3] = 42,
        };
        
        Assert.Equal(expectedResult, product);
    }

    [Fact]
    public void MultiplyMatrixByTuple()
    {
        var a = new Matrix(4, 4)
        {
            [0, 0] = 1,
            [0, 1] = 2,
            [0, 2] = 3,
            [0, 3] = 4,
            [1, 0] = 2,
            [1, 1] = 4,
            [1, 2] = 4,
            [1, 3] = 2,
            [2, 0] = 8,
            [2, 1] = 6,
            [2, 2] = 4,
            [2, 3] = 1,
            [3, 0] = 0,
            [3, 1] = 0,
            [3, 2] = 0,
            [3, 3] = 1,
        };
        var b = new Tuple(1, 2, 3, 1);

        var result = a * b;
        var expectedResult = new Tuple(18, 24, 33, 1);
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void MultiplyMatrixByIdentity_ReturnsSameMatrix()
    {
        var a = new Matrix(4, 4)
        {
            [0, 0] = 0,
            [0, 1] = 1,
            [0, 2] = 2,
            [0, 3] = 4,
            [1, 0] = 1,
            [1, 1] = 2,
            [1, 2] = 4,
            [1, 3] = 8,
            [2, 0] = 2,
            [2, 1] = 4,
            [2, 2] = 8,
            [2, 3] = 16,
            [3, 0] = 4,
            [3, 1] = 8,
            [3, 2] = 16,
            [3, 3] = 32,
        };

        var result = a * Matrix.Identity;

        Assert.Equal(a, result);
    }

    [Fact]
    public void TransposeMatrix()
    {
        var a = new Matrix(4, 4)
        {
            [0, 0] = 0,
            [0, 1] = 9,
            [0, 2] = 3,
            [0, 3] = 0,
            [1, 0] = 9,
            [1, 1] = 8,
            [1, 2] = 0,
            [1, 3] = 8,
            [2, 0] = 1,
            [2, 1] = 8,
            [2, 2] = 5,
            [2, 3] = 3,
            [3, 0] = 0,
            [3, 1] = 0,
            [3, 2] = 5,
            [3, 3] = 8,
        };

        var result = a.Transpose();
        var expectedResult = new Matrix(4, 4)
        {
            [0, 0] = 0,
            [0, 1] = 9,
            [0, 2] = 1,
            [0, 3] = 0,
            [1, 0] = 9,
            [1, 1] = 8,
            [1, 2] = 8,
            [1, 3] = 0,
            [2, 0] = 3,
            [2, 1] = 0,
            [2, 2] = 5,
            [2, 3] = 5,
            [3, 0] = 0,
            [3, 1] = 8,
            [3, 2] = 3,
            [3, 3] = 8,
        };
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void TransposeIdentity_IsIdentity()
    {
        var result = Matrix.Identity.Transpose();
        
        Assert.Equal(Matrix.Identity, result);
    }

    [Fact]
    public void Determinant2X2Matrix()
    {
        var a = new Matrix(2, 2)
        {
            [0, 0] = 1,
            [0, 1] = 5,
            [1, 0] = -3,
            [1, 1] = 2,
        };
        
        var determinant = a.Determinant();
        
        Assert.Equal(17, determinant);
    }

    [Fact]
    public void Submatrix3X3()
    {
        var a = new Matrix(3, 3)
        {
            [0, 0] = 1,
            [0, 1] = 5,
            [0, 2] = 0,
            [1, 0] = -3,
            [1, 1] = 2,
            [1, 2] = 7,
            [2, 0] = 0,
            [2, 1] = 6,
            [2, 2] = -3,
        };

        var submatrix = a.Submatrix(0, 2);
        var expectedResult = new Matrix(2, 2)
        {
            [0, 0] = -3,
            [0, 1] = 2,
            [1, 0] = 0,
            [1, 1] = 6,
        };
        
        Assert.Equal(expectedResult, submatrix);
    }

    [Fact]
    public void Submatrix4X4()
    {
        var a = new Matrix(4, 4)
        {
            [0, 0] = -6,
            [0, 1] = 1,
            [0, 2] = 1,
            [0, 3] = 6,
            [1, 0] = -8,
            [1, 1] = 5,
            [1, 2] = 8,
            [1, 3] = 6,
            [2, 0] = -1,
            [2, 1] = 0,
            [2, 2] = 8,
            [2, 3] = 2,
            [3, 0] = -7,
            [3, 1] = 1,
            [3, 2] = -1,
            [3, 3] = 1,
        };

        var result = a.Submatrix(2, 1);
        var expectedResult = new Matrix(3, 3)
        {
            [0, 0] = -6,
            [0, 1] = 1,
            [0, 2] = 6,
            [1, 0] = -8,
            [1, 1] = 8,
            [1, 2] = 6,
            [2, 0] = -7,
            [2, 1] = -1,
            [2, 2] = 1,
        };
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void Minor3X3Matrix()
    {
        var a = new Matrix(3, 3)
        {
            [0, 0] = 3,
            [0, 1] = 5,
            [0, 2] = 0,
            [1, 0] = 2,
            [1, 1] = -1,
            [1, 2] = -7,
            [2, 0] = 6,
            [2, 1] = -1,
            [2, 2] = 5,
        };
        
        var minor = a.Minor(1, 0);
        
        Assert.Equal(25, minor);
    }

    [Fact]
    public void Cofactor3X3Matrix()
    {
        var a = new Matrix(3, 3)
        {
            [0, 0] = 3,
            [0, 1] = 5,
            [0, 2] = 0,
            [1, 0] = 2,
            [1, 1] = -1,
            [1, 2] = -7,
            [2, 0] = 6,
            [2, 1] = -1,
            [2, 2] = 5,
        };

        var cofactor = a.Cofactor(0, 0);
        Assert.Equal(-12, cofactor);

        cofactor = a.Cofactor(1, 0);
        Assert.Equal(-25, cofactor);
    }

    [Fact]
    public void Determinant3X3Matrix()
    {
        var a = new Matrix(3, 3)
        {
            [0, 0] = 1,
            [0, 1] = 2,
            [0, 2] = 6,
            [1, 0] = -5,
            [1, 1] = 8,
            [1, 2] = -4,
            [2, 0] = 2,
            [2, 1] = 6,
            [2, 2] = 4,
        };

        var determinant = a.Determinant();
        Assert.Equal(-196, determinant);
    }

    [Fact]
    public void Determinant4X4Matrix()
    {
        var a = new Matrix(4, 4)
        {
            [0, 0] = -2,
            [0, 1] = -8,
            [0, 2] = 3,
            [0, 3] = 5,
            [1, 0] = -3,
            [1, 1] = 1,
            [1, 2] = 7,
            [1, 3] = 3,
            [2, 0] = 1,
            [2, 1] = 2,
            [2, 2] = -9,
            [2, 3] = 6,
            [3, 0] = -6,
            [3, 1] = 7,
            [3, 2] = 7,
            [3, 3] = -9,
        };
        
        var determinant = a.Determinant();
        Assert.Equal(-4071, determinant);
    }

    [Theory, MemberData((nameof(IsInvertibleData)))]
    public void IsInvertible(Matrix a, bool isInvertable)
    {
        Assert.Equal(isInvertable, a.IsInvertible());
    }

    public static TheoryData<Matrix, bool> IsInvertibleData => new()
    {
        {
            new Matrix(4, 4)
            {
                [0, 0] = 6,
                [0, 1] = 4,
                [0, 2] = 4,
                [0, 3] = 4,
                [1, 0] = 5,
                [1, 1] = 5,
                [1, 2] = 7,
                [1, 3] = 6,
                [2, 0] = 4,
                [2, 1] = -9,
                [2, 2] = 3,
                [2, 3] = -7,
                [3, 0] = -9,
                [3, 1] = 1,
                [3, 2] = 7,
                [3, 3] = -6,
            },
            true
        },
        {
            new Matrix(4, 4)
            {
                [0, 0] = -4,
                [0, 1] = 2,
                [0, 2] = -2,
                [0, 3] = -3,
                [1, 0] = 9,
                [1, 1] = 6,
                [1, 2] = 2,
                [1, 3] = 6,
                [2, 0] = 0,
                [2, 1] = -5,
                [2, 2] = 1,
                [2, 3] = -5,
                [3, 0] = 0,
                [3, 1] = 0,
                [3, 2] = 0,
                [3, 3] = 0,
            },
            false
        },
    };

    [Theory, MemberData(nameof(InverseMatrixData))]
    public void InverseOfMatrix(Matrix a, Matrix expectedResult)
    {
        var result = a.Inverse();
        
        Assert.Equal(expectedResult, result);
    }

    public static TheoryData<Matrix, Matrix> InverseMatrixData => new()
    {
        {
            new Matrix(4, 4)
            {
                [0, 0] = -5,
                [0, 1] = 2,
                [0, 2] = 6,
                [0, 3] = -8,
                [1, 0] = 1,
                [1, 1] = -5,
                [1, 2] = 1,
                [1, 3] = 8,
                [2, 0] = 7,
                [2, 1] = 7,
                [2, 2] = -6,
                [2, 3] = -7,
                [3, 0] = 1,
                [3, 1] = -3,
                [3, 2] = 7,
                [3, 3] = 4,
            },
            new Matrix(4, 4)
            {
                [0, 0] = 0.21805,
                [0, 1] = 0.45113,
                [0, 2] = 0.24060,
                [0, 3] = -0.04511,
                [1, 0] = -0.80827,
                [1, 1] = -1.45677,
                [1, 2] = -0.44361,
                [1, 3] = 0.52068,
                [2, 0] = -0.07895,
                [2, 1] = -0.22368,
                [2, 2] = -0.05263,
                [2, 3] = 0.19737,
                [3, 0] = -0.52256,
                [3, 1] = -0.81391,
                [3, 2] = -0.30075,
                [3, 3] = 0.30639,
            }
        },
        {
            new Matrix(4, 4)
            {
                [0, 0] = 8,
                [0, 1] = -5,
                [0, 2] = 9,
                [0, 3] = 2,
                [1, 0] = 7,
                [1, 1] = 5,
                [1, 2] = 6,
                [1, 3] = 1,
                [2, 0] = -6,
                [2, 1] = 0,
                [2, 2] = 9,
                [2, 3] = 6,
                [3, 0] = -3,
                [3, 1] = 0,
                [3, 2] = -9,
                [3, 3] = -4,
            },
            new Matrix(4, 4)
            {
                [0, 0] = -0.15385,
                [0, 1] = -0.15385,
                [0, 2] = -0.28205,
                [0, 3] = -0.53846,
                [1, 0] = -0.07692,
                [1, 1] = 0.12308,
                [1, 2] = 0.02564,
                [1, 3] = 0.03077,
                [2, 0] = 0.35897,
                [2, 1] = 0.35897,
                [2, 2] = 0.43590,
                [2, 3] = 0.92308,
                [3, 0] = -0.69231,
                [3, 1] = -0.69231,
                [3, 2] = -0.76923,
                [3, 3] = -1.92308,
            }
        },
        {
            new Matrix(4, 4)
            {
                [0, 0] = 9,
                [0, 1] = 3,
                [0, 2] = 0,
                [0, 3] = 9,
                [1, 0] = -5,
                [1, 1] = -2,
                [1, 2] = -6,
                [1, 3] = -3,
                [2, 0] = -4,
                [2, 1] = 9,
                [2, 2] = 6,
                [2, 3] = 4,
                [3, 0] = -7,
                [3, 1] = 6,
                [3, 2] = 6,
                [3, 3] = 2,
            },
            new Matrix(4, 4)
            {
                [0, 0] = -0.04074,
                [0, 1] = -0.07778,
                [0, 2] = 0.14444,
                [0, 3] = -0.22222,
                [1, 0] = -0.07778,
                [1, 1] = 0.03333,
                [1, 2] = 0.36667,
                [1, 3] = -0.33333,
                [2, 0] = -0.02901,
                [2, 1] = -0.14630,
                [2, 2] = -0.10926,
                [2, 3] = 0.12963,
                [3, 0] = 0.17778,
                [3, 1] = 0.06667,
                [3, 2] = -0.26667,
                [3, 3] = 0.33333,
            }
        }        
    };

    [Fact]
    public void MultiplyProductByInverse()
    {
        var a = new Matrix(4, 4)
        {
            [0, 0] = 3,
            [0, 1] = -9,
            [0, 2] = 7,
            [0, 3] = 3,
            [1, 0] = 3,
            [1, 1] = -8,
            [1, 2] = 2,
            [1, 3] = -9,
            [2, 0] = -4,
            [2, 1] = 4,
            [2, 2] = 4,
            [2, 3] = 1,
            [3, 0] = -6,
            [3, 1] = 5,
            [3, 2] = -1,
            [3, 3] = 1,
        };
        var b = new Matrix(4, 4)
        {
            [0, 0] = 8,
            [0, 1] = 2,
            [0, 2] = 2,
            [0, 3] = 2,
            [1, 0] = 3,
            [1, 1] = -1,
            [1, 2] = 7,
            [1, 3] = 0,
            [2, 0] = 7,
            [2, 1] = 0,
            [2, 2] = 5,
            [2, 3] = 4,
            [3, 0] = 6,
            [3, 1] = -2,
            [3, 2] = 0,
            [3, 3] = 5,
        };

        var c = a * b;
        var result = c * b.Inverse();
        
        Assert.Equal(a, result);
    }
}