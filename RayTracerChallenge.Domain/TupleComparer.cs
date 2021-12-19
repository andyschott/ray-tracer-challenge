using System.Diagnostics.CodeAnalysis;

namespace RayTracerChallenge.Domain;

public class TupleComparer : IEqualityComparer<Tuple>
{
    public bool Equals(Tuple? x, Tuple? y)
    {
        if(x is null && y is null)
        {
            return true;
        }
        if(x is null || y is null)
        {
            return false;
        }

        return x.X == y.X &&
            x.Y == y.Y &&
            x.Z == y.Z &&
            x.W == y.W;
    }

    public int GetHashCode([DisallowNull] Tuple obj)
    {
        return HashCode.Combine(obj.X, obj.Y, obj.Z, obj.W);
    }
}