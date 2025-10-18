using RayTracerChallenge.Domain;
using RayTracerChallenge.Domain.Shapes;
using RayTracerChallenge.IO;
using Tuple = RayTracerChallenge.Domain.Tuple;

namespace RayTracerChallenge.Tests.IO;

public class ObjModelParserTests
{
    private readonly ObjModelParser _parser = new();

    [Fact]
    public void IgnoresGibberish()
    {
        const string input =
            """
            There was a young lady named Bright
            who traveled much faster than light.
            She set out one day
            in a relative way,
            and came back the previous night.
            """;
        using var reader = new StringReader(input);
        
        var model = _parser.Parse(reader);
        
        Assert.Empty(model.Vertices);
    }

    [Fact]
    public void ParsesVertexRecords()
    {
        const string input =
            """
            v -1 1 0
            v -1.0000 0.5000 0.0000
            v 1 0 0
            v 1 1 0
            """;
        using var reader = new StringReader(input);
        
        var model = _parser.Parse(reader);

        Assert.Equal(5, model.Vertices.Count);
        Assert.Equal(Tuple.CreatePoint(-1, 1, 0), model.Vertices[1]);
        Assert.Equal(Tuple.CreatePoint(-1, 0.5, 0), model.Vertices[2]);
        Assert.Equal(Tuple.CreatePoint(1, 0, 0), model.Vertices[3]);
        Assert.Equal(Tuple.CreatePoint(1, 1, 0), model.Vertices[4]);
    }

    [Fact]
    public void ParsingTriangleFaces()
    {
        const string input =
            """
            v -1 1 0
            v -1 0 0
            v 1 0 0
            v 1 1 0
            f 1 2 3
            f 1 3 4
            """;
        using var reader = new StringReader(input);
        
        var model = _parser.Parse(reader);
        
        Assert.Equal(2, model.DefaultGroup.Count);
        var t1 = Assert.IsType<Triangle>(model.DefaultGroup[0]);
        Assert.Equal(model.Vertices[1], t1.Point1);
        Assert.Equal(model.Vertices[2], t1.Point2);
        Assert.Equal(model.Vertices[3], t1.Point3);
        
        var t2 = Assert.IsType<Triangle>(model.DefaultGroup[1]);
        Assert.Equal(model.Vertices[1], t2.Point1);
        Assert.Equal(model.Vertices[3], t2.Point2);
        Assert.Equal(model.Vertices[4], t2.Point3);
    }

    [Fact]
    public void TriangulatingPolygons()
    {
        const string input =
            """
            v -1 1 0
            v -1 0 0
            v 1 0 0
            v 1 1 0
            v 0 2 0
            f 1 2 3 4 5
            """;
        using var reader = new StringReader(input);
        
        var model = _parser.Parse(reader);
        Assert.Equal(3, model.DefaultGroup.Count);
        
        var t1 = Assert.IsType<Triangle>(model.DefaultGroup[0]);
        Assert.Equal(model.Vertices[1], t1.Point1);
        Assert.Equal(model.Vertices[2], t1.Point2);
        Assert.Equal(model.Vertices[3], t1.Point3);
        
        var t2 = Assert.IsType<Triangle>(model.DefaultGroup[1]);
        Assert.Equal(model.Vertices[1], t2.Point1);
        Assert.Equal(model.Vertices[3], t2.Point2);
        Assert.Equal(model.Vertices[4], t2.Point3);
        
        var t3 = Assert.IsType<Triangle>(model.DefaultGroup[2]);
        Assert.Equal(model.Vertices[1], t3.Point1);
        Assert.Equal(model.Vertices[4], t3.Point2);
        Assert.Equal(model.Vertices[5], t3.Point3);
    }

    [Fact]
    public void TrianglesInGroups()
    {
        const string input =
            """
            v -1 1 0
            v -1 0 0
            v 1 0 0
            v 1 1 0
            g FirstGroup
            f 1 2 3
            g SecondGroup
            f 1 3 4
            """;
        using var reader = new StringReader(input);
        
        var model = _parser.Parse(reader);

        var firstGroup = Assert.Contains("FirstGroup", model.Groups);
        Assert.Single(firstGroup);
        var t1 = Assert.IsType<Triangle>(firstGroup[0]);
        Assert.Equal(model.Vertices[1], t1.Point1);
        Assert.Equal(model.Vertices[2], t1.Point2);
        Assert.Equal(model.Vertices[3], t1.Point3);
        
        var secondGroup = Assert.Contains("SecondGroup", model.Groups);
        Assert.Single(secondGroup);
        var t2 = Assert.IsType<Triangle>(secondGroup[0]);
        Assert.Equal(model.Vertices[1], t2.Point1);
        Assert.Equal(model.Vertices[3], t2.Point2);
        Assert.Equal(model.Vertices[4], t2.Point3);

        var g = model.ToGroup(new Color(1, 1, 1));
        Assert.Equal(2, g.Count);
    }

    [Fact]
    public void VertexNormalRecords()
    {
        const string input =
            """
            vn 0 0 1
            vn 0.707 0 -0.707
            vn 1 2 3
            """;
        using var reader = new StringReader(input);
        
        var model = _parser.Parse(reader);
        
        Assert.Equal(4, model.Normals.Count);

        var normal1 = Tuple.CreateVector(0, 0, 1);
        Assert.Equal(normal1, model.Normals[1]);
        
        var normal2 = Tuple.CreateVector(0.707, 0, -0.707);
        Assert.Equal(normal2, model.Normals[2]);
        
        var normal3 =  Tuple.CreateVector(1, 2, 3);
        Assert.Equal(normal3, model.Normals[3]);
    }

    [Fact]
    public void FacesWithNormals()
    {
        const string input =
            """
            v 0 1 0
            v -1 0 0
            v 1 0 0
            vn -1 0 0
            vn 1 0 0
            vn 0 1 0
            f 1//3 2//1 3//2
            f 1/0/3 2/102/1 3/14/2
            """;
        using var reader = new StringReader(input);
        
        var model = _parser.Parse(reader);
        
        Assert.Equal(2, model.DefaultGroup.Count);
        
        var t1 = Assert.IsType<SmoothTriangle>(model.DefaultGroup[0]);
        Assert.Equal(model.Vertices[1], t1.Point1);
        Assert.Equal(model.Vertices[2], t1.Point2);
        Assert.Equal(model.Vertices[3], t1.Point3);
        Assert.Equal(model.Normals[3], t1.Normal1);
        Assert.Equal(model.Normals[1], t1.Normal2);
        Assert.Equal(model.Normals[2], t1.Normal3);
        
        var t2 =  Assert.IsType<SmoothTriangle>(model.DefaultGroup[1]);
        Assert.Equal(t1, t2);
    }
}