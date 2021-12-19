﻿using System.Runtime.CompilerServices;

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
}