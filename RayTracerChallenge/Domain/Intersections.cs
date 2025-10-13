using System.Collections.ObjectModel;

namespace RayTracerChallenge.Domain;

public class Intersections : Collection<Intersection>
{
    public Intersection? Hit()
    {
        return this.Where(i => i.T > 0)
            .OrderBy(i => i.T)
            .FirstOrDefault();
    }
}