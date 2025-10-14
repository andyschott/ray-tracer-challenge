using Tuple = RayTracerChallenge.Domain.Tuple;

namespace RayTracerChallenge.Tests.Domain;

public class TupleTests
{
    [Fact]
    public void PointIsATuple()
    {
        var point = Tuple.CreatePoint(4.3, -4.2, 3.1);

        Assert.True(point.IsPoint);
        Assert.False(point.IsVector);
    }

    [Fact]
    public void PointIsAVector()
    {
        var point = Tuple.CreateVector(4.3, -4.2, 3.1);

        Assert.False(point.IsPoint);
        Assert.True(point.IsVector);
    }

    [Fact]
    public void AddTuples()
    {
        var a1 = Tuple.CreatePoint(3, -2, 5);
        var a2 = Tuple.CreateVector(-2, 3, 1);

        var sum = a1 + a2;
        var expectedResult = Tuple.CreatePoint(1, 1, 6);

        Assert.Equal(expectedResult, sum);
    }

    [Fact]
    public void SubtractPoints()
    {
        var p1 = Tuple.CreatePoint(3, 2, 1);
        var p2 = Tuple.CreatePoint(5, 6, 7);

        var diff = p1 - p2;
        var expectedResult = Tuple.CreateVector(-2, -4, -6);

        Assert.Equal(expectedResult, diff);
    }

    [Fact]
    public void SubtractVectorFromPoint()
    {
        var p = Tuple.CreatePoint(3, 2, 1);
        var v = Tuple.CreateVector(5, 6, 7);

        var diff = p - v;
        var expectedResult = Tuple.CreatePoint(-2, -4, -6);

        Assert.Equal(expectedResult, diff);
    }

    [Fact]
    public void SubtractVectors()
    {
        var v1 = Tuple.CreateVector(3, 2, 1);
        var v2 = Tuple.CreateVector(5, 6, 7);

        var diff = v1 - v2;
        var expectedResult = Tuple.CreateVector(-2, -4, -6);

        Assert.Equal(expectedResult, diff);
    }

    [Fact]
    public void SubtractVectorFromZeroVector()
    {
        var v = Tuple.CreateVector(1, -2, 3);

        var diff = Tuple.ZeroVector - v;
        var expectedResult = Tuple.CreateVector(-1, 2, -3);

        Assert.Equal(expectedResult, diff);
    }

    [Fact]
    public void NegateTuple()
    {
        var a = new Tuple(1, -2, 3, -4);

        var opposite = -a;
        var expectedResult = new Tuple(-1, 2, -3, 4);

        Assert.Equal(expectedResult, opposite);
    }

    [Fact]
    public void MultiplyTupleByScalar()
    {
        var a = new Tuple(1, -2, 3, -4);

        var result = a * 3.5;
        var expectedResult = new Tuple(3.5, -7, 10.5, -14);

        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void MultiplyTupleByFraction()
    {
        var a = new Tuple(1, -2, 3, -4);

        var result = a * 0.5;
        var expectedResult = new Tuple(0.5, -1, 1.5, -2);

        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void DivideTupleByScalar()
    {
        var a = new Tuple(1, -2, 3, -4);

        var result = a / 2;
        var expectedResult = new Tuple(0.5, -1, 1.5, -2);

        Assert.Equal(expectedResult, result);
    }

    [Theory, MemberData(nameof(ComputeMagnitudeOfVectorData))]
    public void ComputeMagnitudeOfVector(double x, double y, double z,
        double expectedResult)
    {
        var v = Tuple.CreateVector(x, y, z);

        Assert.Equal(expectedResult, v.Magnitude, 4);
    }

    public static TheoryData<double, double, double, double> ComputeMagnitudeOfVectorData => new()
    {
        { 1, 0, 0, 1 },
        { 0, 1, 0, 1 },
        {1, 2, 3, 3.7416573868 },
        { -1, -2, -3, 3.7416573868 }
    };

    [Theory, MemberData(nameof(NormalizeVectorData))]
    public void NormalizeVector(Tuple v, Tuple expectedResult)
    {
        var result = v.Normalize();
        
        Assert.Equal(result.X, expectedResult.X, 4);
        Assert.Equal(result.Y, expectedResult.Y, 4);
        Assert.Equal(result.Z, expectedResult.Z, 4);
        Assert.Equal(result.W, expectedResult.W, 4);
    }
    
    public static TheoryData<Tuple, Tuple> NormalizeVectorData => new()
    {
        {
            Tuple.CreateVector(4, 0, 0),
            Tuple.CreateVector(1, 0, 0)
        },
        {
            Tuple.CreateVector(1, 2, 3),
            Tuple.CreateVector(0.2672612419,
                0.5345224838,
                0.8017837257)
        }
    };

    [Fact]
    public void MagnitudeOfNormalizedVector()
    {
        var v = Tuple.CreateVector(1, 2, 3);
        var normalized = v.Normalize();
        
        Assert.Equal(1, normalized.Magnitude);
    }

    [Fact]
    public void DotProductOfVectors()
    {
        var a = Tuple.CreateVector(1, 2, 3);
        var b = Tuple.CreateVector(2, 3, 4);

        var dot = a.Dot(b);

        Assert.Equal(20, dot);
    }

    [Fact]
    public void CrossProductOfVectors()
    {
        var a = Tuple.CreateVector(1, 2, 3);
        var b = Tuple.CreateVector(2, 3, 4);

        var cross = a.Cross(b);
        
        var expectedResult = Tuple.CreateVector(-1, 2, -1);
        Assert.Equal(expectedResult, cross);

        cross = b.Cross(a);
        expectedResult = Tuple.CreateVector(1, -2, 1);
        Assert.Equal(expectedResult, cross);
    }

    [Fact]
    public void ReflectingVectorApproachingAt45Degrees()
    {
        var v = Tuple.CreateVector(1, -1, 0);
        var n = Tuple.CreateVector(0, 1, 0);

        var r = v.Reflect(n);
        var expectedResult = Tuple.CreateVector(1, 1, 0);
        
        Assert.Equal(expectedResult, r);
    }

    [Fact]
    public void RelfectingVectorOffOfSlantedSurface()
    {
        var v = Tuple.CreateVector(0, -1, 0);
        var n = Tuple.CreateVector(Math.Sqrt(2)/2,
            Math.Sqrt(2)/2, 0);

        var r = v.Reflect(n);
        var expectedResult = Tuple.CreateVector(1, 0, 0);
        
        Assert.Equal(expectedResult, r);
    }
}
