using System;
using System.Linq;
using AutoFixture.Kernel;

namespace RayTracerChallenge.Domain.Tests;

public class ShapeTests
{
    private readonly IFixture _fixture = new Fixture();

    private readonly TransformationFactory _factory = new TransformationFactory();

    private readonly IntersectionComparer _intersectionComparer = new IntersectionComparer();
    private readonly MatrixComparer _matrixComparer = new MatrixComparer();
    private readonly TupleComparer _tupleComparer = new TupleComparer();
    private readonly MaterialComparer _materialComparer = new MaterialComparer();

    public ShapeTests()
    {
        _fixture.Customizations.Add(new TypeRelay(typeof(Shape), typeof(TestShape)));
        _fixture.Customize<TestShape>(c => c.OmitAutoProperties());
    }

    [Fact]
    public void ChangeSphereTransformation()
    {
        var transform = _factory.Translation(2, 3, 4);
        var shape = _fixture.Build<TestShape>()
            .With(shape => shape.Transform, transform)
            .OmitAutoProperties()
            .Create();

        Assert.Same(transform, shape.Transform);
    }

    [Fact]
    public void DefaultMaterial()
    {
        var shape = _fixture.Create<Shape>();

        var expected = new Material();

        Assert.Equal(expected, shape.Material, _materialComparer);
    }

    [Fact]
    public void CustomMaterial()
    {
        var material = _fixture.Create<Material>();
        var shape = _fixture.Build<TestShape>()
            .With(shape => shape.Material, material)
            .OmitAutoProperties()
            .Create();

        Assert.Equal(material, shape.Material, _materialComparer);
    }
}
