using System.Collections.Generic;

namespace RayTracerChallenge.Domain.Tests;

public class TestShape : Shape
{
    protected override IEnumerable<Intersection> LocalIntersects(Ray ray)
    {
        throw new System.NotImplementedException();
    }

    protected override Tuple LocalNormalAt(Tuple point)
    {
        throw new System.NotImplementedException();
    }
}
