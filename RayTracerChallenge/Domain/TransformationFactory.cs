using RayTracerChallenge.Extensions;

namespace RayTracerChallenge.Domain;

public static class TransformationFactory
{
    public static Matrix Translation(double x,
        double y,
        double z)
    {
        return new Matrix(Matrix.Identity)
        {
            [0, 3] = x,
            [1, 3] = y,
            [2, 3] = z
        };
    }

    public static Matrix Scale(double x,
        double y,
        double z)
    {
        return new Matrix(Matrix.Identity)
        {
            [0, 0] = x,
            [1, 1] = y,
            [2, 2] = z
        };
    }

    public static Matrix RotationX(double radians)
    {
        var cos = Math.Cos(radians);
        var sin = Math.Sin(radians);

        return new Matrix(Matrix.Identity)
        {
            [1, 1] = cos,
            [1, 2] = -sin,
            [2, 1] = sin,
            [2, 2] = cos,
        };
    }

    public static Matrix RotationY(double radians)
    {
        var cos = Math.Cos(radians);
        var sin = Math.Sin(radians);

        return new Matrix(Matrix.Identity)
        {
            [0, 0] = cos,
            [0, 2] = sin,
            [2, 0] = -sin,
            [2, 2] = cos,
        };
    }

    public static Matrix RotationZ(double radians)
    {
        var cos = Math.Cos(radians);
        var sin = Math.Sin(radians);

        return new Matrix(Matrix.Identity)
        {
            [0, 0] = cos,
            [0, 1] = -sin,
            [1, 0] = sin,
            [1, 1] = cos,
        };
    }

    public static Matrix Shearing(double xToy,
        double xToz,
        double yToX,
        double yToz,
        double zToX,
        double zToY)
    {
        return new Matrix(Matrix.Identity)
        {
            [0, 1] = xToy,
            [0, 2] = xToz,
            [1, 0] = yToX,
            [1, 2] = yToz,
            [2, 0] = zToX,
            [2, 1] = zToY
        };
    }

    public static Matrix View(Tuple from,
        Tuple to,
        Tuple up)
    {
        var forward = (to - from).Normalize();
        var normalUp = up.Normalize();
        var left = forward.Cross(normalUp);
        var trueUp = left.Cross(forward);

        var orientation = new Matrix(4, 4)
        {
            [0, 0] = left.X,
            [0, 1] = left.Y,
            [0, 2] = left.Z,
            [0, 3] = 0,
            [1, 0] = trueUp.X,
            [1, 1] = trueUp.Y,
            [1, 2] = trueUp.Z,
            [1, 3] = 0,
            [2, 0] = -forward.X,
            [2, 1] = -forward.Y,
            [2, 2] = -forward.Z,
            [2, 3] = 0,
            [3, 0] = 0,
            [3, 1] = 0,
            [3, 2] = 0,
            [3, 3] = 1,
        };
        return orientation *
            Translation(-from.X, -from.Y, -from.Z);
    }
}