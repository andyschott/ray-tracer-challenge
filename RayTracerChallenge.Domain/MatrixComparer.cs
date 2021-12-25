using System.Diagnostics.CodeAnalysis;

namespace RayTracerChallenge.Domain;

public class MatrixComparer : IEqualityComparer<Matrix>
{
    private const decimal Epsilon = 0.0001M;

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
                if(Math.Abs(first[y, x] - second[y, x]) > Epsilon)
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
