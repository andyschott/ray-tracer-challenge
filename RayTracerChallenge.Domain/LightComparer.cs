using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("RayTracerChallenge.Domain.Tests")]

namespace RayTracerChallenge.Domain;

public class LightComparer : IEqualityComparer<Light>
{
    private readonly IEqualityComparer<Tuple> _tupleComparer;
    private readonly IEqualityComparer<Color> _colorComparer;

    public LightComparer()
        : this(new TupleComparer(), new ColorComparer())
    {
    }

    // Should be called directly for testing purposes only
    internal LightComparer(IEqualityComparer<Tuple> tupleComparer, IEqualityComparer<Color> colorComparer)
    {
        _tupleComparer = tupleComparer;
        _colorComparer = colorComparer;
    }

    public bool Equals(Light? x, Light? y)
    {
        if(x is null && y is null)
        {
            return true;
        }
        if(x is null || y is null)
        {
            return false;
        }

        return _tupleComparer.Equals(x.Position, y.Position) &&
            _colorComparer.Equals(x.Intensity, y.Intensity);
    }

    public int GetHashCode([DisallowNull] Light obj)
    {
        return HashCode.Combine(_tupleComparer.GetHashCode(obj.Position),
            _colorComparer.GetHashCode(obj.Intensity));
    }
}
