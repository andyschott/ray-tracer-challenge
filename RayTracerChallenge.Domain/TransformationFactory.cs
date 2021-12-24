namespace RayTracerChallenge.Domain;

public class TransformationFactory
{
    public Matrix Translation(Tuple amount)
    {
        if(!amount.IsPoint)
        {
            throw new ArgumentException($"{amount} must be a point", nameof(amount));
        }

        var matrix = Matrix.IdentityMatrix(4, 4);
        matrix[0,3] = amount.X;
        matrix[1,3] = amount.Y;
        matrix[2,3] = amount.Z;

        return matrix;
    }

    public Matrix Scale(float x, float y, float z)
    {
        var matrix = Matrix.IdentityMatrix(4, 4);
        matrix[0,0] = x;
        matrix[1,1] = y;
        matrix[2,2] = z;

        return matrix;
    }
}