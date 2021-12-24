using Math = System.Math;

namespace RayTracerChallenge.Domain;

public class TransformationFactory
{
    public Matrix Translation(float x, float y, float z)
    {
        var matrix = Matrix.Identity(4, 4);
        matrix[0,3] = x;
        matrix[1,3] = y;
        matrix[2,3] = z;

        return matrix;
    }

    public Matrix Scale(float x, float y, float z)
    {
        var matrix = Matrix.Identity(4, 4);
        matrix[0,0] = x;
        matrix[1,1] = y;
        matrix[2,2] = z;

        return matrix;
    }

    public Matrix RotationAroundXAxis(double radians)
    {
        var cos = (float)Math.Cos(radians);
        var sin = (float)Math.Sin(radians);

        var matrix = Matrix.Identity(4, 4);
        matrix[1,1] = cos;
        matrix[1,2] = -1 * sin;
        matrix[2,1] = sin;
        matrix[2,2] = cos;

        return matrix;
    }
    
    public Matrix RotationAroundYAxis(double radians)
    {
        var cos = (float)Math.Cos(radians);
        var sin = (float)Math.Sin(radians);

        var matrix = Matrix.Identity(4, 4);
        matrix[0,0] = cos;
        matrix[0,2] = sin;
        matrix[2,0] = -1 * sin;
        matrix[2,2] = cos;

        return matrix;
    }

    public Matrix RotationAroundZAxis(double radians)
    {
        var cos = (float)Math.Cos(radians);
        var sin = (float)Math.Sin(radians);

        var matrix = Matrix.Identity(4, 4);
        matrix[0,0] = cos;
        matrix[0,1] = -1 * sin;
        matrix[1,0] = sin;
        matrix[1,1] = cos;
        
        return matrix;
    }

    public Matrix Shearing(float xToY, float xToZ,
        float yToX, float yToZ,
        float zToX, float zToY)
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
}