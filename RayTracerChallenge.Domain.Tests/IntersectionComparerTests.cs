namespace RayTracerChallenge.Domain.Tests;

public class IntersectionComparerTests : AbstractComparerTests<Intersection>
{
    public IntersectionComparerTests() : base(new IntersectionComparer())
    {
    }

    [Fact]
    public void SameValuesAreEqual()
    {
        var t = _fixture.Create<decimal>();
        var sphere = _fixture.Create<Sphere>();

        var intersection1 = new Intersection(t, sphere);
        var intersection2 = new Intersection(t, sphere);

        var result = _comparer.Equals(intersection1, intersection2);

        Assert.True(result);
    }
}
