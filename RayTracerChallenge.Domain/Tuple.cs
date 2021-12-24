using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("RayTracerChallenge.Domain.Tests")]

namespace RayTracerChallenge.Domain;

public record class Tuple
{
    public float X { get; init; }
    public float Y { get; init; }
    public float Z { get; init; }
    public float W { get; init; }

    public bool IsPoint => W == Point;
    public bool IsVector => W == Vector;

    internal const float Point = 1.0F;
    internal const float Vector = 0.0F;

    public Tuple()
    {
    }

    public Tuple(float x, float y, float z, float w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

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
            W = W + vector.W
        };
    }

    public static Tuple operator +(Tuple x, Tuple y) => x.Add(y);

    public Tuple Subtract(Tuple other)
    {
        if(IsVector && other.IsPoint)
        {
            throw new ArgumentException($"Cannot subtract a point from a vector", nameof(other));
        }

        return new Tuple
        {
            X = X - other.X,
            Y = Y - other.Y,
            Z = Z - other.Z,
            W = W - other.W
        };
    }

    public static Tuple operator -(Tuple x, Tuple y) => x.Subtract(y);

    public Tuple Negate()
    {
        return new Tuple
        {
            X = X * -1,
            Y = Y * -1,
            Z = Z * -1,
            W = W * -1
        };
    }

    public static Tuple operator -(Tuple x) => x.Negate();

    public Tuple Multiply(float factor)
    {
        return new Tuple
        {
            X = X * factor,
            Y = Y * factor,
            Z = Z * factor,
            W = W * factor
        };
    }

    public static Tuple operator *(Tuple x, float factor) => x.Multiply(factor);

    public Tuple Divide(float factor)
    {
        if(factor == 0.0F)
        {
            throw new DivideByZeroException();
        }

        return new Tuple
        {
            X = X / factor,
            Y = Y / factor,
            Z = Z / factor,
            W = W / factor
        };
    }

    public static Tuple operator /(Tuple x, float factor) => x.Divide(factor);

    public float Magnitude()
    {
        var sum = Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2) + Math.Pow(W, 2);
        return (float)Math.Sqrt(sum);
    }

    public Tuple Normalize()
    {
        var magntiude = Magnitude();
        return new Tuple
        {
            X = X / magntiude,
            Y = Y / magntiude,
            Z = Z / magntiude,
            W = W / magntiude
        };
    }

    public float DotProduct(Tuple other)
    {
        return X * other.X +
            Y * other.Y +
            Z * other.Z +
            W * other.W;
    }

    public Tuple CrossProduct(Tuple other)
    {
        if(!IsVector)
        {
            throw new Exception("Can only compute cross product of vectors");
        }
        if(!other.IsVector)
        {
            throw new ArgumentException("Can only compute cross product of vectors", nameof(other));
        }

        return CreateVector(Y * other.Z - Z * other.Y,
            Z * other.X - X * other.Z,
            X * other.Y - Y * other.X);
    }

    public override string ToString() => $"({X}, {Y}, {Z}, {W}";
}
