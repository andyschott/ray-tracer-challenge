using System.Text;
using RayTracerChallenge.Extensions;

namespace RayTracerChallenge.Domain;

public sealed record Tuple
{
    public double X { get; init; }
    public double Y { get; init; }
    public double Z { get; init; }
    public double W { get; init; }
    
    public bool IsPoint => W is 1.0;
    public bool IsVector => W is 0.0;
    
    public static Tuple ZeroVector { get; } =
        Tuple.CreateVector(0, 0, 0);

    public Tuple(double x, double y, double z, double w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }
    
    public static Tuple CreatePoint(double x, double y, double z)
        => new(x, y, z, 1.0);
    public static Tuple CreateVector(double x, double y, double z)
        => new(x, y, z, 0.0);

    public double Magnitude
    {
        get
        {
            var x = X * X;
            var y = Y * Y;
            var z = Z * Z;
            var w = W * W;
            var sum = x + y + z + w;

            return Math.Sqrt(sum);
        }
    }

    public Tuple Normalize()
    {
        var magnitude = Magnitude;
        return new Tuple(X / magnitude,
            Y / magnitude,
            Z / magnitude,
            W / magnitude);
    }

    public double Dot(Tuple other)
    {
        return X * other.X +
               Y * other.Y +
               Z * other.Z +
               W * other.W;
    }

    public Tuple Cross(Tuple other)
    {
        return Tuple.CreateVector(Y * other.Z - Z * other.Y,
            Z * other.X - X * other.Z,
            X * other.Y - Y * other.X);
    }

    public Tuple Reflect(Tuple normal)
    {
        return this - normal * 2 * Dot(normal);
    }
    
    public static Tuple operator +(Tuple left, Tuple right)
    {
        return new Tuple(left.X + right.X,
            left.Y + right.Y,
            left.Z + right.Z,
            left.W + right.W);
    }

    public static Tuple operator -(Tuple left, Tuple right)
    {
        return new Tuple(left.X - right.X,
            left.Y - right.Y,
            left.Z - right.Z,
            left.W - right.W);
    }

    public static Tuple operator -(Tuple tuple)
    {
        return new Tuple(-tuple.X, -tuple.Y, -tuple.Z, -tuple.W);
    }

    public static Tuple operator *(Tuple left, double right)
    {
        return new Tuple(left.X * right,
            left.Y * right,
            left.Z * right,
            left.W * right);
    }

    public static Tuple operator /(Tuple left, double right)
    {
        return new Tuple(left.X / right,
            left.Y / right,
            left.Z / right,
            left.W / right);
    }

    public bool Equals(Tuple? other)
    {
        if (other is null)
        {
            return false;
        }
        
        if (ReferenceEquals(null, other))
        {
            return true;
        }
        
        return X.IsEquivalent(other.X) &&
               Y.IsEquivalent(other.Y) &&
               Z.IsEquivalent(other.Z) &&
               W.IsEquivalent(other.W);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z, W);
    }

    private bool PrintMembers(StringBuilder builder)
    {
        builder.Append($"X = {X}, Y = {Y}, Z = {Z}, W = {W}");
        return true;
    }
}
    