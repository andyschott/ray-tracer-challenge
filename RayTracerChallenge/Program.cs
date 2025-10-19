using System.Diagnostics;
using RayTracerChallenge.Domain;
using RayTracerChallenge.Domain.Shapes;
using RayTracerChallenge.Extensions;
using RayTracerChallenge.IO;
using Tuple = RayTracerChallenge.Domain.Tuple;
/*
var floor = new Plane
{
    Material = new Material
    {
        Color = new Color(0.133333, 0.36, 0.85),
        Specular = 0,
        Reflective = 1,
        RefractiveIndex = 1.333,
        Transparency = 1.5,
    }
};

var wall = new Plane(Matrix.Identity.RotateX(Math.PI / 2)
    .Translate(0, 0, 5))
{
    Material = new Material
    {
        Color = new Color(0, 0, 1),
        Specular = 0
    }
};

var middle = new Sphere(Matrix.Identity.Translate(-0.5, 1, 0.5))
{
    Material = new Material
    {
        Color = new Color(1, 0, 1),
        Diffuse = 0.7,
        Specular = 0.3
    }
};

var right = new Sphere(Matrix.Identity.Scale(0.5, 0.5, 0.5)
    .Translate(1.5, 0.5, -0.5))
{
    Material = middle.Material
};

var left = new Sphere(Matrix.Identity.Scale(0.33, 0.33, 0.33)
    .Translate(-1.5, 0.33, -0.75))
{
    Material = middle.Material
};

var cube = new Cube(Matrix.Identity.Translate(-4, 1, 0.5))
{
    Material = new Material
    {
        Color = new Color(1, 1, 0),
        Diffuse = 0.7,
        Specular = 0.3,
    }
};

var cylinder = new Cylinder(Matrix.Identity.Translate(4, 0, 0.5))
{
    Material = new Material
    {
        Color = new Color(0.2, 0.41, 0),
        Ambient = 1
    },
    Minimum = 0,
    Maximum = 2,
    Closed = true
};
*/
var world = new World
{
    Light = new PointLight(Tuple.CreatePoint(-15, 10, -15),
        new Color(1, 1, 1))
};
// world.Shapes.Add(floor);
// world.Shapes.Add(wall);
// world.Shapes.Add(middle);
// world.Shapes.Add(right);
// world.Shapes.Add(left);
// world.Shapes.Add(cube);
// world.Shapes.Add(cylinder);
var ground = new Plane
{
    Material = new Material
    {
        Color = new Color(0.76, 0.7, 0.50),
    }
};

var sky = new Plane(Matrix.Identity
    .RotateX(Math.PI / 4)
    .Translate(0, 0, 100))
{
    Material = new Material
    {
        Color = new Color(0.53, 0.81, 0.92),
    }
};

world.Shapes.Add(ground);
world.Shapes.Add(sky);
world.Shapes.Add(Pyramid(Matrix.Identity.Scale(3, 3, 3)));

// var group = BuildGroup();
// if (group is null)
// {
//     return;
// }
// world.Shapes.Add(group);

var camera = new Camera(2000, 1000, Math.PI / 3,
    TransformationFactory.View(Tuple.CreatePoint(3, 5, -15),
        Tuple.CreatePoint(0, 1, 0),
        Tuple.CreatePoint(0, 1, 0)));

Log("Tracing rays...");
var stopWatch = Stopwatch.StartNew();
var canvas = camera.Render(world);
stopWatch.Stop();
Log($"Done. Took {stopWatch.ElapsedMilliseconds}ms");

Log("Saving canvas to disk...");
await using var writer = new StreamWriter("output.ppm");
new CanvasPpmSerializer()
    .Serialize(canvas, writer);
Log("Done.");

return;

static void Log(string str)
{
    Console.WriteLine($"[{DateTime.Now}] {str}");
}

Group? BuildGroup()
{
    string inputFile;
    if (args.Length > 0)
    {
        inputFile = args[0];
    }
    else
    {
        Console.WriteLine("Path to Input File:");
        Console.Write("> ");
    
        var response = Console.ReadLine();
        if (string.IsNullOrEmpty(response))
        {
            return null;
        }

        inputFile = response;
    }
    
    Console.Write("Red: ");
    var r = double.Parse(Console.ReadLine()!);
    Console.Write("Green: ");
    var g = double.Parse(Console.ReadLine()!);
    Console.Write("Blue: ");
    var b = double.Parse(Console.ReadLine()!);
    
    using var reader = new StreamReader(inputFile);
    var parser = new ObjModelParser();
    var model = parser.Parse(reader);
    return model.ToGroup(new Color(r, g, b),
        Matrix.Identity.Scale(3, 3, 3));
}

static Shape HexagonCorner()
{
    return new Sphere(Matrix.Identity.Scale(0.25, 0.25, 0.25)
        .Translate(0, 0, -1));
}

static Shape HexagonEdge()
{
    var edge = new Cylinder(Matrix.Identity.Scale(0.25, 1, 0.25)
        .RotateZ(-1 * Math.PI / 2)
        .RotateY(-1 * Math.PI / 6)
        .Translate(0, 0, -1))
    {
        Minimum = 0,
        Maximum = 1
    };

    return edge;
}

static Shape HexagonSide(Matrix transform)
{
    var side = new Group(transform)
    {
        HexagonCorner(),
        HexagonEdge()
    };
    return side;
}

static Shape Hexagon(Matrix transform)
{
    var hexagon = new Group(transform);
    for (var n = 0; n < 6; n++)
    {
        var side = HexagonSide(Matrix.Identity.RotateY(n * (Math.PI / 3)));
        hexagon.Add(side);
    }
    
    return hexagon;
}

static Shape Pyramid(Matrix transform)
{
    var material = new Material
    {
        Color = new Color(1, 0.65, 0)
    };
    var t1 = new Triangle(-1, 0, -1,
        1, 0, -1,
        0, 1, 0)
    {
        Material = material
    };
    var t2 = new Triangle(-1, 0, 1,
        -1, 0, -1,
        0, 1, 0)
    {
        Material = material
    };
    var t3 = new Triangle(1, 0, 1,
        -1, 0, 1,
        0, 1, 0)
    {
        Material = material
    };
    var t4 = new Triangle(1, 0, 1,
        1, 0, -1,
        0, 1, 0)
    {
        Material = material
    };

    var pyramid = new Group(transform)
    {
        t1,
        t2,
        t3,
        t4
    };

    return pyramid;
}
