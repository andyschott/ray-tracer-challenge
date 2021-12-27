using System.Collections.Generic;
using IDisposable = System.IDisposable;

namespace RayTracerChallenge.Domain.Tests;

public sealed class LightComparerTests : IDisposable
{
    private readonly MockRepository _repository = new MockRepository(MockBehavior.Strict);
    private readonly IFixture _fixture = new Fixture();

    private readonly Mock<IEqualityComparer<Tuple>> _mockTupleComparer;
    private readonly Mock<IEqualityComparer<Color>> _mockColorComparer;

    private readonly LightComparer _lightComparer;

    public LightComparerTests()
    {
        _fixture.Register(() => new Light(_fixture.Build<Tuple>()
            .With(t => t.W, Tuple.Point)
            .Create(),
            _fixture.Create<Color>()));

        _mockTupleComparer = _repository.Create<IEqualityComparer<Tuple>>();
        _mockColorComparer = _repository.Create<IEqualityComparer<Color>>();

        _lightComparer = new LightComparer(_mockTupleComparer.Object,
            _mockColorComparer.Object);
    }

    public void Dispose()
    {
        _repository.Verify();
        _repository.VerifyNoOtherCalls();
    }

    [Fact]
    public void BothObjectsNull()
    {
        var result = _lightComparer.Equals(null, null);

        Assert.True(result);
    }

    [Fact]
    public void FirstObjectNull()
    {
        var result = _lightComparer.Equals(null, _fixture.Create<Light>());

        Assert.False(result);
    }

    [Fact]
    public void SecondObjectNull()
    {
        var result = _lightComparer.Equals(_fixture.Create<Light>(), null);

        Assert.False(result);
    }

    [Fact]
    public void ObjectsAreEqual()
    {
        var x = _fixture.Create<Light>();
        var y = _fixture.Create<Light>();

        _mockTupleComparer.Setup(comparer => comparer.Equals(x.Position, y.Position))
            .Returns(true)
            .Verifiable();
        _mockColorComparer.Setup(comparer => comparer.Equals(x.Intensity, y.Intensity))
            .Returns(true)
            .Verifiable();

        var result = _lightComparer.Equals(x, y);
        
        Assert.True(result);
    }

    [Fact]
    public void PositionsAreDifferent()
    {
        var x = _fixture.Create<Light>();
        var y = _fixture.Create<Light>();

        _mockTupleComparer.Setup(comparer => comparer.Equals(x.Position, y.Position))
            .Returns(false)
            .Verifiable();

        var result = _lightComparer.Equals(x, y);

        Assert.False(result);
    }

    [Fact]
    public void IntensitiesAreDifferent()
    {
        var x = _fixture.Create<Light>();
        var y = _fixture.Create<Light>();

        _mockTupleComparer.Setup(comparer => comparer.Equals(x.Position, y.Position))
            .Returns(true)
            .Verifiable();
        _mockColorComparer.Setup(comparer => comparer.Equals(x.Intensity, y.Intensity))
            .Returns(false)
            .Verifiable();

        var result = _lightComparer.Equals(x, y);

        Assert.False(result);
    }
}
