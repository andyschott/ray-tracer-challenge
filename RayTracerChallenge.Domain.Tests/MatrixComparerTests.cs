using System.Collections.Generic;

namespace RayTracerChallenge.Domain.Tests;

public class MatrixComparerTests : AbstractComparerTests<Matrix>
{
    public MatrixComparerTests() : base(new MatrixComparer())
    {
    }

    [Fact]
    public void SameValuesAreEqual()
    {
        var data = new decimal[4,4];
        data[0,0] = 1;
        data[0,1] = 2;
        data[0,2] = 3;
        data[0,3] = 4;
        data[1,0] = 5;
        data[1,1] = 6;
        data[1,2] = 7;
        data[1,3] = 8;
        data[2,0] = 9;
        data[2,1] = 8;
        data[2,2] = 7;
        data[2,3] = 6;
        data[3,0] = 5;
        data[3,1] = 4;
        data[3,2] = 3;
        data[3,3] = 2;

        var matrix1 = new Matrix(data);
        var matrix2 = new Matrix(data);

        Assert.Equal(matrix1, matrix2, _comparer);
    }
}
