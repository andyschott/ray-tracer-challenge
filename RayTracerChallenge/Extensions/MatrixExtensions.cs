using RayTracerChallenge.Domain;

namespace RayTracerChallenge.Extensions;

public static class MatrixExtensions
{
    public static Matrix RotateX(this Matrix matrix, double angle)
    {
        var transform = TransformationFactory.RotationX(angle);
        return transform * matrix;
    }

    public static Matrix RotateY(this Matrix matrix, double angle)
    {
        var transform = TransformationFactory.RotationY(angle);
        return transform * matrix;
    }

    public static Matrix RotateZ(this Matrix matrix, double angle)
    {
        var transform = TransformationFactory.RotationZ(angle);
        return transform * matrix;
    }

    public static Matrix Scale(this Matrix matrix,
        decimal x, decimal y, decimal z)
    {
        var transform = TransformationFactory.Scale(x, y, z);
        return transform * matrix;
    }

    public static Matrix Translate(this Matrix matrix,
        decimal x, decimal y, decimal z)
    {
        var transform = TransformationFactory.Translation(x, y, z);
        return transform * matrix;
    }

    public static Matrix Shearing(this Matrix matrix,
        decimal xToy,
        decimal xToz,
        decimal yToX,
        decimal yToz,
        decimal zToX,
        decimal zToY)
    {
        var transform = TransformationFactory.Shearing(
            xToy, xToz, yToX, yToz, zToX, zToY);
        return transform * matrix;
    }
}