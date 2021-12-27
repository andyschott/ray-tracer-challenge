using System.Diagnostics.CodeAnalysis;

namespace RayTracerChallenge.Domain;

public class IntersectionComparer : IEqualityComparer<Intersection>
{
    private static decimal Epsilon = 0.0001M;

    public bool Equals(Intersection? x, Intersection? y)
    {
        if(x is null && y is null)
        {
            return true;
        }
        if(x is null || y is null)
        {
            return false;
        }

        return Math.Abs(x.T - y.T) < Epsilon &&
            ReferenceEquals(x.Object, y.Object);
    }

    public int GetHashCode([DisallowNull] Intersection obj)
    {
        return HashCode.Combine(obj.T, obj.Object);
    }
}
