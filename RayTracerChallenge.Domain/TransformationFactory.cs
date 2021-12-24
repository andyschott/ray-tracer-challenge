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
}