using System.Diagnostics.CodeAnalysis;

namespace RayTracerChallenge.Domain;

public class MaterialComparer : IEqualityComparer<Material>
{
    private readonly IEqualityComparer<Color> _colorComparer;

    private const decimal Epsilon = 0.0001M;

    public MaterialComparer()
        : this(new ColorComparer())
    {
    }

    // Called directly for testing only
    internal MaterialComparer(IEqualityComparer<Color> colorComparer)
    {
        _colorComparer = colorComparer;
    }

    public bool Equals(Material? x, Material? y)
    {
        if(x is null && y is null)
        {
            return true;
        }
        if(x is null || y is null)
        {
            return false;
        }

        return _colorComparer.Equals(x.Color, y.Color) &&
            Math.Abs(x.Ambient - y.Ambient) < Epsilon &&
            Math.Abs(x.Diffuse - y.Diffuse) < Epsilon &&
            Math.Abs(x.Specular - y.Specular) < Epsilon &&
            Math.Abs(x.Shininess - y.Shininess) < Epsilon &&
            ReferenceEquals(x.Pattern, y.Pattern); // TODO: consider writing pattern comparers
    }

    public int GetHashCode([DisallowNull] Material obj)
    {
        return HashCode.Combine(_colorComparer.GetHashCode(obj.Color),
            obj.Ambient, obj.Diffuse, obj.Specular,
            obj.Shininess);
    }
}
