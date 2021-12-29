namespace RayTracerChallenge.Domain.Tests.Extensions;

public static class FixtureExtensions
{
    public static Tuple CreatePoint(this IFixture fixture)
    {
        return fixture.Build<Tuple>()
            .With(tuple => tuple.W, Tuple.Point)
            .Create();
    }

    public static Tuple CreateVector(this IFixture fixture)
    {
        return fixture.Build<Tuple>()
            .With(tuple => tuple.W, Tuple.Vector)
            .Create();
    }

    public static Light CreateLight(this IFixture fixture)
    {
        return new Light(fixture.CreatePoint(),
            fixture.Create<Color>());
    }
}
