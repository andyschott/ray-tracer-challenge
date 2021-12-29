using AutoFixture.Kernel;

namespace RayTracerChallenge.Domain.Tests;

public class IntersectionComparerTests : AbstractComparerTests<Intersection>
{
    public IntersectionComparerTests() : base(new IntersectionComparer())
    {
        _fixture.Customizations.Add(new TypeRelay(typeof(Shape), typeof(TestShape)));
        _fixture.Customize<Material>(c => c.Without(m => m.Pattern));
    }

    [Fact]
    public void SameValuesAreEqual()
    {
        var t = _fixture.Create<decimal>();
        var sphere = _fixture.Create<Shape>();

        var intersection1 = new Intersection(t, sphere);
        var intersection2 = new Intersection(t, sphere);

        var result = _comparer.Equals(intersection1, intersection2);

        Assert.True(result);
    }
}
