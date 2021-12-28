using Math = System.Math;

namespace RayTracerChallenge.Domain;

public class TransformationFactory
{
    public Matrix Translation(decimal x, decimal y, decimal z)
    {
        var matrix = Matrix.Identity(4, 4);
        matrix[0,3] = x;
        matrix[1,3] = y;
        matrix[2,3] = z;

        return matrix;
    }

    public Matrix Scale(decimal x, decimal y, decimal z)
    {
        var matrix = Matrix.Identity(4, 4);
        matrix[0,0] = x;
        matrix[1,1] = y;
        matrix[2,2] = z;

        return matrix;
    }

    public Matrix RotationAroundXAxis(double radians)
    {
        var cos = (decimal)Math.Cos(radians);
        var sin = (decimal)Math.Sin(radians);

        var matrix = Matrix.Identity(4, 4);
        matrix[1,1] = cos;
        matrix[1,2] = -1 * sin;
        matrix[2,1] = sin;
        matrix[2,2] = cos;

        return matrix;
    }
    
    public Matrix RotationAroundYAxis(double radians)
    {
        var cos = (decimal)Math.Cos(radians);
        var sin = (decimal)Math.Sin(radians);

        var matrix = Matrix.Identity(4, 4);
        matrix[0,0] = cos;
        matrix[0,2] = sin;
        matrix[2,0] = -1 * sin;
        matrix[2,2] = cos;

        return matrix;
    }

    public Matrix RotationAroundZAxis(double radians)
    {
        var cos = (decimal)Math.Cos(radians);
        var sin = (decimal)Math.Sin(radians);

        var matrix = Matrix.Identity(4, 4);
        matrix[0,0] = cos;
        matrix[0,1] = -1 * sin;
        matrix[1,0] = sin;
        matrix[1,1] = cos;
        
        return matrix;
    }

    public Matrix Shearing(decimal xToY, decimal xToZ,
        decimal yToX, decimal yToZ,
        decimal zToX, decimal zToY)
    {
        var matrix = Matrix.Identity(4, 4);
        matrix[0,1] = xToY;
        matrix[0,2] = xToZ;
        matrix[1,0] = yToX;
        matrix[1,2] = yToZ;
        matrix[2,0] = zToX;
        matrix[2,1] = zToY;

        return matrix;
    }

    public Matrix TransformView(Tuple from, Tuple to, Tuple up)
    {
        if(!from.IsPoint)
        {
            throw new ArgumentException($"{nameof(from)} must be a point");
        }
        if(!to.IsPoint)
        {
            throw new ArgumentException($"{nameof(to)} must be a point");
        }
        if(!up.IsVector)
        {
            throw new ArgumentException($"{nameof(up)} must be a vector");
        }

        var forward = (to - from).Normalize();
        var normalizedUp = up.Normalize();
        var left = forward.CrossProduct(normalizedUp);
        var trueUp = left.CrossProduct(forward);

        var orientation = new Matrix(4, 4,
            left.X,      left.Y,     left.Z,    0,
            trueUp.X,    trueUp.Y,   trueUp.Z,  0,
            -forward.X, -forward.Y, -forward.Z, 0,
             0,          0,          0,         1);

        var translation = Translation(-from.X, -from.Y, -from.Z);
        return orientation * translation;
    }
}