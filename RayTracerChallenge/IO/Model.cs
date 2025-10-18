using RayTracerChallenge.Domain;
using RayTracerChallenge.Domain.Shapes;
using Tuple = RayTracerChallenge.Domain.Tuple;

namespace RayTracerChallenge.IO;

public record Model
{
    private const string DefaultGroupName = "default";

    public List<Tuple> Vertices { get; }
    public List<Tuple> Normals { get; }
    public Dictionary<string, Group> Groups { get; }
    
    public Group DefaultGroup => Groups[DefaultGroupName];
    
    public Model(List<Tuple> vertices,
        List<Tuple> normals,
        Dictionary<string, Group> groups,
        Group defaultGroup)
    {
        Vertices = vertices;
        Normals = normals;
        Groups = groups;
        Groups.Add(DefaultGroupName, defaultGroup);
    }

    public Group ToGroup(Color color,
        Matrix? transform = null)
    {
        var g = new Group(transform)
        {
            Material = new Material
            {
                Color = color,
            }
        };
        foreach (var group in Groups.Values.Where(group => group.Count > 0))
        {
            g.Add(group);
        }

        return g;
    }
}