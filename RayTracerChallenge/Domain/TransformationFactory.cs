namespace RayTracerChallenge.Domain;

public static class TransformationFactory
{
    public static Matrix Translation(decimal x,
        decimal y,
        decimal z)
    {
        return new Matrix(Matrix.Identity)
        {
            [0, 3] = x,
            [1, 3] = y,
            [2, 3] = z
        };
    }

    public static Matrix Scale(decimal x,
        decimal y,
        decimal z)
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
        var cos = (decimal)Math.Cos(radians);
        var sin = (decimal)Math.Sin(radians);

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
        var cos = (decimal)Math.Cos(radians);
        var sin = (decimal)Math.Sin(radians);

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
        var cos = (decimal)Math.Cos(radians);
        var sin = (decimal)Math.Sin(radians);

        return new Matrix(Matrix.Identity)
        {
            [0, 0] = cos,
            [0, 1] = -sin,
            [1, 0] = sin,
            [1, 1] = cos,
        };
    }

    public static Matrix Shearing(decimal xToy,
        decimal xToz,
        decimal yToX,
        decimal yToz,
        decimal zToX,
        decimal zToY)
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
}