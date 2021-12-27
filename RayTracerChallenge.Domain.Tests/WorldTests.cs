using System.Linq;

namespace RayTracerChallenge.Domain.Tests;

public class WorldTests
{
    private readonly LightComparer _lightComparer = new LightComparer();

    [Fact]
    public void EmptyWorld()
    {
        var world = new World();

        Assert.Null(world.Light);
        Assert.Empty(world.Objects);
    }

    [Fact]
    public void VerifyDefaultWorld()
    {
        var world = World.Default();

        var expectedLight = new Light(Tuple.CreatePoint(-10, 10, -10),
            new Color
            {
                Red = 1,
                Green = 1,
                Blue = 1
            });

        Assert.NotNull(world.Light);
        if(world.Light is not null) // to make the compiler happy
        {
            Assert.Equal(expectedLight, world.Light, _lightComparer);
        }

        Assert.Equal(2, world.Objects.Count());
    }

    [Fact]
    public void IntersectWorldWithRay()
    {
        var world = World.Default();
        var ray = new Ray(Tuple.CreatePoint(0, 0, -5), Tuple.CreateVector(0, 0, 1));

        var intersections = world.Intersect(ray)
            .ToArray();

        Assert.Equal(4, intersections.Length);
        Assert.Equal(4, intersections[0].T);
        Assert.Equal(4.5M, intersections[1].T);
        Assert.Equal(5.5M, intersections[2].T);
        Assert.Equal(6, intersections[3].T);
    }
}
