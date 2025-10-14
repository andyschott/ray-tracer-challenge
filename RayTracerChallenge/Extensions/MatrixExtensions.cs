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
        double x, double y, double z)
    {
        var transform = TransformationFactory.Scale(x, y, z);
        return transform * matrix;
    }

    public static Matrix Translate(this Matrix matrix,
        double x, double y, double z)
    {
        var transform = TransformationFactory.Translation(x, y, z);
        return transform * matrix;
    }

    public static Matrix Shearing(this Matrix matrix,
        double xToy,
        double xToz,
        double yToX,
        double yToz,
        double zToX,
        double zToY)
    {
        var transform = TransformationFactory.Shearing(
            xToy, xToz, yToX, yToz, zToX, zToY);
        return transform * matrix;
    }
}