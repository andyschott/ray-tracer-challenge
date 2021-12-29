using System.Diagnostics.CodeAnalysis;
using RayTracerChallenge.Domain.Patterns;

namespace RayTracerChallenge.Domain;

public class MaterialComparer : IEqualityComparer<Material>
{
    private const decimal Epsilon = 0.0001M;

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

        // TODO: consider writing pattern comparers
        return Math.Abs(x.Ambient - y.Ambient) < Epsilon &&
            Math.Abs(x.Diffuse - y.Diffuse) < Epsilon &&
            Math.Abs(x.Specular - y.Specular) < Epsilon &&
            Math.Abs(x.Shininess - y.Shininess) < Epsilon; 
    }

    public int GetHashCode([DisallowNull] Material obj)
    {
        return HashCode.Combine(obj.Ambient, obj.Diffuse, obj.Specular,
            obj.Shininess);
    }
}
