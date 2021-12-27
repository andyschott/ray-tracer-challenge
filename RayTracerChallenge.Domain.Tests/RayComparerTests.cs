namespace RayTracerChallenge.Domain.Tests;

public class RayComparerTests : AbstractComparerTests<Ray>
{
    public RayComparerTests() : base(new RayComparer())
    {
        _fixture.Register(() => new Ray(
            _fixture.Build<Tuple>()
                .With(t => t.W, Tuple.Point)
                .Create(),
            _fixture.Build<Tuple>()
                .With(t => t.W, Tuple.Vector)
                .Create()
        ));
    }

    [Fact]
    public void SameValuesAreEqual()
    {
        var origin = _fixture.Build<Tuple>()
            .With(t => t.W, Tuple.Point)
            .Create();
        var direction = _fixture.Build<Tuple>()
            .With(t => t.W, Tuple.Vector)
            .Create();

        var ray1 = new Ray(origin, direction);
        var ray2 = new Ray(origin, direction);

        var result = _comparer.Equals(ray1, ray2);

        Assert.True(result);
    }
}
