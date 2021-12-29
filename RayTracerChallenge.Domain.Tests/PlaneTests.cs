using System.Linq;
using RayTracerChallenge.Domain.Tests.Extensions;

namespace RayTracerChallenge.Domain.Tests;

public class PlaneTests
{
    private readonly IFixture _fixture = new Fixture();

    private readonly TupleComparer _tupleComparer = new TupleComparer();

    public PlaneTests()
    {
        _fixture.Customize<Plane>(c => c.OmitAutoProperties());
    }

    [Fact]
    public void NormalOfPlaneIsConstantEverywhere()
    {
        var plane = new Plane();

        var result = plane.NormalAt(_fixture.CreatePoint());

        var expectedResult = Tuple.CreateVector(0, 1, 0);
        Assert.Equal(expectedResult, result, _tupleComparer);
    }

    [Theory]
    [InlineData(0, 10, 0)]
    [InlineData(0, 0, 0)]
    public void RayDoesNotIntersectPlane(decimal x, decimal y, decimal z)
    {
        var plane = new Plane();
        var ray = new Ray(Tuple.CreatePoint(x, y, z), Tuple.CreateVector(0, 0, 1));

        var result = plane.Intersects(ray);
        
        Assert.Empty(result);
    }

    [Theory]
    [InlineData(0, 1, 0, 0, -1, 0)]
    [InlineData(0, -1, 0, 0, 1, 0)]
    public void RayIntersectingPlane(decimal pointX, decimal pointY, decimal pointZ,
        decimal vectorX, decimal vectorY, decimal vectorZ)
    {
        var plane = new Plane();
        var ray = new Ray(Tuple.CreatePoint(pointX, pointY, pointZ), Tuple.CreateVector(vectorX, vectorY, vectorZ));

        var result = plane.Intersects(ray);

        Assert.Single(result);
        Assert.Equal(1, result.First().T);
        Assert.Same(plane, result.First().Object);
    }
}
