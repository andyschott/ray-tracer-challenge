using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("RayTracerChallenge.Domain.Tests")]

namespace RayTracerChallenge.Domain;

public record class Tuple
{
    public decimal X { get; init; }
    public decimal Y { get; init; }
    public decimal Z { get; init; }
    public decimal W { get; init; }

    public bool IsPoint => W == Point;
    public bool IsVector => W == Vector;

    internal const decimal Point = 1;
    internal const decimal Vector = 0;

    public Tuple()
    {
    }

    public Tuple(decimal x, decimal y, decimal z, decimal w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    public static Tuple CreatePoint(decimal x, decimal y, decimal z)
    {
        return new Tuple
        {
            X = x,
            Y = y,
            Z = z,
            W = Point
        };
    }

    public static Tuple CreateVector(decimal x, decimal y, decimal z)
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

    public Tuple Multiply(decimal factor)
    {
        return new Tuple
        {
            X = X * factor,
            Y = Y * factor,
            Z = Z * factor,
            W = W * factor
        };
    }

    public static Tuple operator *(Tuple x, decimal factor) => x.Multiply(factor);

    public Tuple Divide(decimal factor)
    {
        if(factor == 0)
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

    public static Tuple operator /(Tuple x, decimal factor) => x.Divide(factor);

    public decimal Magnitude()
    {
        var sum = Math.Pow((double)X, 2) + Math.Pow((double)Y, 2) + Math.Pow((double)Z, 2) + Math.Pow((double)W, 2);
        return (decimal)Math.Sqrt(sum);
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

    public decimal DotProduct(Tuple other)
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

    public override string ToString() => $"({X}, {Y}, {Z}, {W})";
}
