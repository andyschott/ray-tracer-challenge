namespace RayTracerChallenge.Domain.Extensions;

public static class IntersectionExtensions
{
    public static Intersection? Hit(this IEnumerable<Intersection> intersections)
    {
        return intersections?.Where(intersection => intersection.T > 0)
            .OrderBy(intersection => intersection.T)
            .FirstOrDefault();
    }
}
