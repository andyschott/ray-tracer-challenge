using RayTracerChallenge.Domain;
using RayTracerChallenge.Extensions;
using Tuple = RayTracerChallenge.Domain.Tuple;

namespace RayTracerChallenge.Tests.Domain;

public class TransformationFactoryTests
{
    [Fact]
    public void MultiplyByTranslationMatrix()
    {
        var transform = TransformationFactory.Translation(5, -3, 2);
        var p = Tuple.CreatePoint(-3, 4, 5);

        var result = transform * p;
        var expectedResult = Tuple.CreatePoint(2, 1, 7);
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void MultiplyByInverseOfTranslationMatrix()
    {
        var transform = TransformationFactory.Translation(5, -3, 2);
        var inverse = transform.Inverse();
        var p = Tuple.CreatePoint(-3, 4, 5);
        
        var result = inverse * p;
        var expectedResult = Tuple.CreatePoint(-8, 7, 3);
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void TranslationDoesNotAffectVectors()
    {
        var transform = TransformationFactory.Translation(5, -3, 2);
        var v = Tuple.CreateVector(-3, 4, 5);
        
        var result = transform * v;
        Assert.Equal(v, result);
    }

    [Fact]
    public void ScalePoint()
    {
        var transform = TransformationFactory.Scale(2, 3, 4);
        var p = Tuple.CreatePoint(-4, 6, 8);
        
        var result = transform * p;
        var expectedResult = Tuple.CreatePoint(-8, 18, 32);
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void ScaleVector()
    {
        var transform = TransformationFactory.Scale(2, 3, 4);
        var v = Tuple.CreateVector(-4, 6, 8);
        
        var result = transform * v;
        var expectedResult = Tuple.CreateVector(-8, 18, 32);
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void MultiplyByInverseOfScalingMatrix()
    {
        var transform = TransformationFactory.Scale(2, 3, 4);
        var inverse = transform.Inverse();
        var v = Tuple.CreateVector(-4, 6, 8);
        
        var result = inverse * v;
        var expectedResult = Tuple.CreateVector(-2, 2, 2);
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void ReflectionIsScalingByNegative()
    {
        var transform = TransformationFactory.Scale(-1, 1, 1);
        var p = Tuple.CreatePoint(2, 3, 4);
        
        var result = transform * p;
        var expectedResult = Tuple.CreatePoint(-2, 3, 4);
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void XRotatePoint()
    {
        var p = Tuple.CreatePoint(0, 1, 0);
        var halfQuarter = TransformationFactory.RotationX(Math.PI / 4);
        var fullQuarter = TransformationFactory.RotationX(Math.PI / 2);
        
        var result = halfQuarter * p;
        var expectedResult = Tuple.CreatePoint(0,
            Math.Sqrt(2) / 2,
            Math.Sqrt(2) / 2);
        Assert.Equal(expectedResult, result);
        
        result = fullQuarter * p;
        expectedResult = Tuple.CreatePoint(0, 0, 1);
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void InverseXRotation()
    {
        var p = Tuple.CreatePoint(0, 1, 0);
        var halfQuarter = TransformationFactory.RotationX(Math.PI / 4);
        var inverse = halfQuarter.Inverse();
        
        var result = inverse * p;
        var expectedResult = Tuple.CreatePoint(0,
            Math.Sqrt(2) / 2,
            -Math.Sqrt(2) / 2);
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void YRotatePoint()
    {
        var p = Tuple.CreatePoint(0, 0, 1);
        var halfQuarter = TransformationFactory.RotationY(Math.PI / 4);
        var fullQuarter = TransformationFactory.RotationY(Math.PI / 2);
        
        var result = halfQuarter * p;
        var expectedResult = Tuple.CreatePoint(Math.Sqrt(2) / 2,
            0,
            Math.Sqrt(2) / 2);
        Assert.Equal(expectedResult, result);
        
        result = fullQuarter * p;
        expectedResult = Tuple.CreatePoint(1, 0, 0);
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void ZRotatePoint()
    {
        var p = Tuple.CreatePoint(0, 1, 0);
        var halfQuarter = TransformationFactory.RotationZ(Math.PI / 4);
        var fullQuarter = TransformationFactory.RotationZ(Math.PI / 2);
        
        var result = halfQuarter * p;
        var expectedResult = Tuple.CreatePoint(-Math.Sqrt(2) / 2,
            Math.Sqrt(2) / 2,
            0);
        Assert.Equal(expectedResult, result);
        
        result = fullQuarter * p;
        expectedResult = Tuple.CreatePoint(-1, 0, 0);
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(1, 0, 0, 0, 0, 0, 5, 3, 4)]
    [InlineData(0, 1, 0, 0, 0, 0, 6, 3, 4)]
    [InlineData(0, 0, 1, 0, 0, 0, 2, 5, 4)]
    [InlineData(0, 0, 0, 1, 0, 0, 2, 7, 4)]
    [InlineData(0, 0, 0, 0, 1, 0, 2, 3, 6)]
    [InlineData(0, 0, 0, 0, 0, 1, 2, 3, 7)]
    public void ShearingXInProportionToY(
        double xToY, double xToZ,
        double yToX, double yToZ,
        double zToX, double zToY,
        double expectedX,
        double expectedY,
        double expectedZ)
    {
        var transform = TransformationFactory.Shearing(xToY,
            xToZ,
            yToX,
            yToZ,
            zToX,
            zToY);
        var p = Tuple.CreatePoint(2, 3, 4);
        
        var result = transform * p;
        var expectedResult = Tuple.CreatePoint(expectedX, expectedY, expectedZ);
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void IndividualTransformsAppliedInSequence()
    {
        var p = Tuple.CreatePoint(1, 0, 1);
        var a = TransformationFactory.RotationX(Math.PI / 2);
        var b = TransformationFactory.Scale(5, 5, 5);
        var c = TransformationFactory.Translation(10, 5, 7);
        
        // Apply rotation first
        var result = a * p;
        var expectedResult = Tuple.CreatePoint(1, -1, 0);
        Assert.Equal(expectedResult, result);
        
        // Then apply scaling
        result = b * result;
        expectedResult = Tuple.CreatePoint(5, -5, 0);
        Assert.Equal(expectedResult, result);
        
        // Then apply translation
        result = c * result;
        expectedResult = Tuple.CreatePoint(15, 0, 7);
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void ChainedTransformationsAppliedInReverse()
    {
        var p = Tuple.CreatePoint(1, 0, 1);
        var t = Matrix.Identity
            .RotateX(Math.PI / 2)
            .Scale(5, 5, 5)
            .Translate(10, 5, 7);
        var result = t * p;
        var expectedResult = Tuple.CreatePoint(15, 0, 7);
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void ViewTransformationForDefaultOrientation()
    {
        var from = Tuple.CreatePoint(0, 0, 0);
        var to = Tuple.CreatePoint(0, 0, -1);
        var up = Tuple.CreateVector(0, 1, 0);
        
        var result = TransformationFactory.View(from, to, up);
        
        Assert.Equal(Matrix.Identity, result);
    }

    [Fact]
    public void ViewTransformationLookingInPositiveZ()
    {
        var from = Tuple.CreatePoint(0, 0, 0);
        var to = Tuple.CreatePoint(0, 0, 1);
        var up = Tuple.CreateVector(0, 1, 0);
        
        var result = TransformationFactory.View(from, to, up);
        var expectedResult = Matrix.Identity.Scale(-1, 1, -1);
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void ViewTransformationMovesTheWorld()
    {
        var from = Tuple.CreatePoint(0, 0, 8);
        var to = Tuple.CreatePoint(0, 0, 0);
        var up = Tuple.CreateVector(0, 1, 0);
        
        var result = TransformationFactory.View(from, to, up);
        var expectedResult = Matrix.Identity.Translate(0, 0, -8);
        
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void ArbitraryViewTransformation()
    {
        var from = Tuple.CreatePoint(1, 3, 2);
        var to = Tuple.CreatePoint(4, -2, 8);
        var up = Tuple.CreateVector(1, 1, 0);
        
        var result = TransformationFactory.View(from, to, up);
        var expectedResult = new Matrix(4, 4)
        {
            [0, 0] = -0.50709,
            [0, 1] = 0.50709,
            [0, 2] = 0.67612,
            [0, 3] = -2.36643,
            [1, 0] = 0.76772,
            [1, 1] = 0.60609,
            [1, 2] = 0.12122,
            [1, 3] = -2.82843,
            [2, 0] = -0.35857,
            [2, 1] = 0.597617,
            [2, 2] = -0.71714,
            [2, 3] = 0,
            [3, 0] = 0,
            [3, 1] = 0,
            [3, 2] = 0,
            [3, 3] = 1,
        };

        Assert.Equal(expectedResult, result);
    }
}