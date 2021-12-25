namespace RayTracerChallenge.Domain.Extensions;

public static class MatrixExtensions
{
    private static TransformationFactory _transformationFactory = new TransformationFactory();

    public static Matrix Translate(this Matrix m, decimal x, decimal y, decimal z)
    {
        var translate = _transformationFactory.Translation(x, y, z);
        return translate * m;
    }

    public static Matrix Scale(this Matrix m, decimal x, decimal y, decimal z)
    {
        var scale = _transformationFactory.Scale(x, y, z);
        return scale * m;
    }

    public static Matrix RotateAroundXAxis(this Matrix m, double radians)
    {
        var rotation = _transformationFactory.RotationAroundXAxis(radians);
        return rotation * m;
    }

    public static Matrix RotateAroundYAxis(this Matrix m, double radians)
    {
        var rotation = _transformationFactory.RotationAroundYAxis(radians);
        return rotation * m;
    }

    public static Matrix RotateAroundZAxis(this Matrix m, double radians)
    {
        var rotation = _transformationFactory.RotationAroundZAxis(radians);
        return rotation * m;
    }

    public static Matrix Shear(this Matrix m, decimal xToY, decimal xToZ,
        decimal yToX, decimal yToZ,
        decimal zToX, decimal zToY)
    {
        var shear = _transformationFactory.Shearing(xToY, xToZ, yToX, yToZ,
            zToX, zToY);
        return shear * m;
    }
}
