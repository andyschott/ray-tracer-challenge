using System.Diagnostics.CodeAnalysis;

namespace RayTracerChallenge.Domain;

public class RayComparer : IEqualityComparer<Ray>
{
    private readonly TupleComparer _tupleComparer = new TupleComparer();

    public bool Equals(Ray? x, Ray? y)
    {
        if(x is null && y is null)
        {
            return true;
        }
        if(x is null || y is null)
        {
            return false;
        }

        return _tupleComparer.Equals(x.Origin, y.Origin) &&
            _tupleComparer.Equals(x.Direction, y.Direction);
    }

    public int GetHashCode([DisallowNull] Ray obj)
    {
        return HashCode.Combine(_tupleComparer.GetHashCode(obj.Origin),
            _tupleComparer.GetHashCode(obj.Direction));
    }
}
