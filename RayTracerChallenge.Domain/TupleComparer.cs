using System.Diagnostics.CodeAnalysis;

namespace RayTracerChallenge.Domain;

public class TupleComparer : IEqualityComparer<Tuple>
{
    private const decimal Epsilon = 0.0001M;
    
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

        return Math.Abs(x.X - y.X) < Epsilon &&
            Math.Abs(x.Y - y.Y) < Epsilon &&
            Math.Abs(x.Z - y.Z) < Epsilon &&
            Math.Abs(x.W - y.W) < Epsilon;
    }

    public int GetHashCode([DisallowNull] Tuple obj)
    {
        return HashCode.Combine(obj.X, obj.Y, obj.Z, obj.W);
    }
}