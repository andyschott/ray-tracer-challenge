using RayTracerChallenge.Domain.Shapes;
using Tuple = RayTracerChallenge.Domain.Tuple;

namespace RayTracerChallenge.IO;

public class ObjModelParser : IModelParser
{
    public Model Parse(TextReader reader)
    {
        var vertices = new List<Tuple>
        {
            Tuple.CreatePoint(0, 0, 0)
        };
        var normals = new List<Tuple>
        {
            Tuple.CreateVector(0, 0, 0)
        };
        var defaultGroup = new Group();
        var currentGroup = defaultGroup;
        var groups = new Dictionary<string, Group>();

        string? line = null;
        while ((line = reader.ReadLine()) is not null)
        {
            var tokens = line.Split(' ');
            if (tokens.Length is < 2)
            {
                continue;
            }
            
            var command = tokens[0];
            switch (command)
            {
                case "v":
                {
                    var args = Parse<double>(tokens.Skip(1));
                    var vertex = ParseVertex(args);
                    if (vertex is not null)
                    {
                        vertices.Add(vertex);
                    }
                    break;
                }
                case "vn":
                {
                    var args = Parse<double>(tokens.Skip(1));
                    var vertex = ParseNormal(args);
                    if (vertex is not null)
                    {
                        normals.Add(vertex);
                    }
                    break;
                }
                case "f":
                {
                    var faces = ParseFace(tokens.AsSpan(1), vertices, normals);
                    foreach (var face in faces)
                    {
                        currentGroup.Add(face);
                    }
                    break;
                }
                case "g":
                {
                    currentGroup = [];
                    groups.Add(tokens[1], currentGroup);
                    break;
                }
            }
        }

        if (vertices.Count is 1)
        {
            vertices.Clear();
        }
        
        return new Model(vertices, normals, groups, defaultGroup);
    }

    private static Tuple? ParseVertex(double[] args)
    {
        if (args.Length is not 3)
        {
            return null;
        }

        return Tuple.CreatePoint(args[0], args[1], args[2]);
    }

    private static Tuple? ParseNormal(double[] args)
    {
        if (args.Length is not 3)
        {
            return null;
        }

        return Tuple.CreateVector(args[0], args[1], args[2]);
    }

    private static List<Shape> ParseFace(Span<string> args,
        List<Tuple> vertices,
        List<Tuple> normals)
    {
        if (args.Length is 0)
        {
            return [];
        }

        var triangles = new List<Shape>();

        var faceArgs = ParseFaceArgs(args);
        var useSmoothTriangles = faceArgs.All(arg => arg.NormalIndex is not null);
        for (var index = 1; index < faceArgs.Length - 1; index++)
        {
            Triangle triangle;
            if (useSmoothTriangles)
            {
                triangle = new SmoothTriangle(vertices[faceArgs[0].VertexIndex],
                    vertices[faceArgs[index].VertexIndex],
                    vertices[faceArgs[index+1].VertexIndex],
                    normals[faceArgs[0].NormalIndex!.Value],
                    normals[faceArgs[index].NormalIndex!.Value],
                    normals[faceArgs[index+1].NormalIndex!.Value]);
            }
            else
            {
                triangle = new Triangle(vertices[faceArgs[0].VertexIndex],
                    vertices[faceArgs[index].VertexIndex],
                    vertices[faceArgs[index+1].VertexIndex]);
            }
            
            triangles.Add(triangle);
        }
        
        return triangles;
    }

    private static T[] Parse<T>(IEnumerable<string> tokens)
    where T : struct, IParsable<T>
    {
        var args = tokens.Select(token =>
        {
            if (!T.TryParse(token, null, out var arg))
            {
                return (T?)null;
            }

            return arg;
        }).Where(arg => arg.HasValue)
        .Select(arg => arg!.Value)
        .ToArray();

        return args;
    }

    private static (int VertexIndex, int? NormalIndex)[] ParseFaceArgs(Span<string> args)
    {
        var faceArgs = new (int VertexIndex, int ? NormalIndex)[args.Length];

        for (var index = 0; index < args.Length; index++)
        {
            if (int.TryParse(args[index], out var vertexIndex))
            {
                faceArgs[index] = (vertexIndex, null);
                continue;
            }
            
            var parts = args[index].Split('/');
            if (parts.Length is not 3)
            {
                continue;
            }

            if (!int.TryParse(parts[0], out vertexIndex) ||
                !int.TryParse(parts[2], out var normalIndex))
            {
                continue;
            }
            
            faceArgs[index] = (vertexIndex, normalIndex);
        }
        
        return faceArgs;
    }
}