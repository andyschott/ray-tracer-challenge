using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("RayTracerChallenge.Domain.Tests")]

namespace RayTracerChallenge.Domain;

public class Tuple
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
    public float W { get; set; }

    public bool IsPoint => W == Point;
    public bool IsVector => W == Vector;

    internal const float Point = 1.0F;
    internal const float Vector = 0.0F;

    public static Tuple CreatePoint(float x, float y, float z)
    {
        return new Tuple
        {
            X = x,
            Y = y,
            Z = z,
            W = Point
        };
    }

    public static Tuple CreateVector(float x, float y, float z)
    {
        return new Tuple
        {
            X = x,
            Y = y,
            Z = z,
            W = Vector
        };
    }

    public Tuple Add(Tuple vector)
    {
        if(!vector.IsVector)
        {
            throw new ArgumentException($"{nameof(vector)} must be a vector", nameof(vector));
        }

        return new Tuple
        {
            X = X + vector.X,
            Y = Y + vector.Y,
            Z = Z + vector.Z,
            W = W
        };
    }

    public static Tuple operator +(Tuple x, Tuple y) => x.Add(y);
}
