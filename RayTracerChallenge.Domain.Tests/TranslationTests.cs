namespace RayTracerChallenge.Domain.Tests;

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
}
