using System.Collections.ObjectModel;

namespace RayTracerChallenge.Domain;

public class Intersections : Collection<Intersection>
{
    public Intersections()
    {
    }

    public Intersections(IList<Intersection> intersections)
    : base(intersections)
    {
    }
    
    public Intersection? Hit()
    {
        return Hits()
            .OrderBy(i => i.T)
            .FirstOrDefault();
    }

    public IEnumerable<Intersection> Hits()
    {
        return this.Where(i => i.T > 0);
    }
}