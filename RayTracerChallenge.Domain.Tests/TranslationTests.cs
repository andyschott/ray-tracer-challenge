namespace RayTracerChallenge.Domain.Tests;

using Math = System.Math;

public class TranslationTests
{
    private readonly TransformationFactory _factory = new TransformationFactory();
    private readonly TupleComparer _tupleComparer = new TupleComparer();

    [Fact]
    public void TranslatePoint()
    {
        var transform = _factory.Translation(Tuple.CreatePoint(5.0F, -3.0F, 2.0F));
        var point = Tuple.CreatePoint(-3.0F, 4.0F, 5.0F);

        var result = transform * point;

        var expectedResult = Tuple.CreatePoint(2.0F, 1.0F, 7.0F);
        Assert.Equal(expectedResult, result, _tupleComparer);
    }

    [Fact]
    public void InverseTranslatePoint()
    {
        var transform = _factory.Translation(Tuple.CreatePoint(5.0F, -3.0F, 2.0F));
        var inverseTransform = transform.Invert();
        var point = Tuple.CreatePoint(-3.0F, 4.0F, 5.0F);
        
        var result = inverseTransform * point;

        var expectedResult = Tuple.CreatePoint(-8.0F, 7.0F, 3.0F);
        Assert.Equal(expectedResult, result, _tupleComparer);
    }

    [Fact]
    public void TranslateVectorDoesNothing()
    {
        var transform = _factory.Translation(Tuple.CreatePoint(5.0F, -3.0F, 2.0F));
        var vector = Tuple.CreateVector(-3.0F, 4.0F, 5.0F);

        var result = transform * vector;

        Assert.Equal(vector, result, _tupleComparer);
    }

    [Fact]
    public void ScalePoint()
    {
        var transform = _factory.Scale(2.0F, 3.0F, 4.0F);
        var point = Tuple.CreatePoint(-4.0F, 6.0F, 8.0F);

        var result = transform * point;

        var expectedResult = Tuple.CreatePoint(-8.0F, 18.0F, 32.0F);
        Assert.Equal(expectedResult, result, _tupleComparer);
    }

    [Fact]
    public void ScaleVector()
    {
        var transform = _factory.Scale(2.0F, 3.0F, 4.0F);
        var vector = Tuple.CreateVector(-4.0F, 6.0F, 8.0F);

        var result = transform * vector;

        var expectedResult = Tuple.CreateVector(-8.0F, 18.0F, 32.0F);
        Assert.Equal(expectedResult, result, _tupleComparer);
    }

    [Fact]
    public void ScaleByInverse()
    {
        var transform = _factory.Scale(2.0F, 3.0F, 4.0F);
        var inverse = transform.Invert();
        var vector = Tuple.CreateVector(-4.0F, 6.0F, 8.0F);

        var result = inverse * vector;

        var expectedResult = Tuple.CreateVector(-2.0F, 2.0F, 2.0F);
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void ReflectPoint()
    {
        var transform = _factory.Scale(-1.0F, 1.0F, 1.0F);
        var point = Tuple.CreatePoint(2.0F, 3.0F, 4.0F);

        var result = transform * point;

        var expectedResult = Tuple.CreatePoint(-2.0F, 3.0F, 4.0F);
        Assert.Equal(expectedResult, result, _tupleComparer);
    }

    [Theory]
    [InlineData(Math.PI / 4, 0.0F, 0.7071067812F, 0.7071067812F)]
    [InlineData(Math.PI / 2, 0.0F, 0.0F, 1.0F)]
    public void RotatePointAroundXAxis(double radians, float expectedX, float expectedY, float expectedZ)
    {
        var point = Tuple.CreatePoint(0.0F, 1.0F, 0.0F);
        var rotation = _factory.RotationAroundXAxis(radians);

        var result = rotation * point;

        var expectedResult = Tuple.CreatePoint(expectedX, expectedY, expectedZ);
        Assert.Equal(expectedResult, result, _tupleComparer);
    }

    [Fact]
    public void InverseRotationAroundXAxis()
    {
        var point = Tuple.CreatePoint(0.0F, 1.0F, 0.0F);
        var rotation = _factory.RotationAroundXAxis(Math.PI / 4);
        var inverse = rotation.Invert();

        var result = inverse * point;

        var expectedResult = Tuple.CreatePoint(0.0F, 0.7071067812F, -0.7071067812F);
        Assert.Equal(expectedResult, result, _tupleComparer);
    }

    [Theory]
    [InlineData(Math.PI / 4, 0.7071067812F, 0.0F, 0.7071067812F)]
    [InlineData(Math.PI / 2, 1.0F, 0.0F, 0.0F)]
    public void RotatePointAroundYAxis(double radians, float expectedX, float expectedY, float expectedZ)
    {
        var point = Tuple.CreatePoint(0.0F, 0.0F, 1.0F);
        var rotation = _factory.RotationAroundYAxis(radians);

        var result = rotation * point;

        var expectedResult = Tuple.CreatePoint(expectedX, expectedY, expectedZ);
        Assert.Equal(expectedResult, result, _tupleComparer);
    }

    [Theory]
    [InlineData(Math.PI / 4, -0.7071067812F, 0.7071067812F, 0.0F)]
    [InlineData(Math.PI / 2, -1.0F, 0.0F, 0.0F)]
    public void RotatePointAroundZAxis(double radians, float expectedX, float expectedY, float expectedZ)
    {
        var point = Tuple.CreatePoint(0.0F, 1.0F, 0.0F);
        var rotation = _factory.RotationAroundZAxis(radians);

        var result = rotation * point;

        var expectedResult = Tuple.CreatePoint(expectedX, expectedY, expectedZ);
        Assert.Equal(expectedResult, result, _tupleComparer);
    }
}
