using System.Diagnostics.CodeAnalysis;

namespace RayTracerChallenge.Domain;

public class MatrixComparer : IEqualityComparer<Matrix>
{
    public bool Equals(Matrix? first, Matrix? second)
    {
        if(first is null && second is null)
        {
            return true;
        }
        if(first is null || second is null)
        {
            return false;
        }

        if(first.Width != second.Width || first.Height != second.Height)
        {
            return false;
        }

        for(var y = 0; y < first.Height; ++y)
        {
            for(var x = 0; x < second.Width; ++x)
            {
                if(first[y, x] != second[y, x])
                {
                    return false;
                }
            }
        }

        return true;
    }

    public int GetHashCode([DisallowNull] Matrix obj)
    {
        throw new NotImplementedException();
    }
}
