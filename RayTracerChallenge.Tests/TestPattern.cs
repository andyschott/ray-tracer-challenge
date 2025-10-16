using RayTracerChallenge.Domain;
using Tuple = RayTracerChallenge.Domain.Tuple;

namespace RayTracerChallenge.Tests;

public record TestPattern : Pattern
{
    public TestPattern(Matrix? transform = null)
    : base(transform)
    {
    }
    
    public override Color ColorAt(Tuple point)
    {
        return new Color(point.X, point.Y, point.Z);
    }
}