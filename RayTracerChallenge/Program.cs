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
    Light = new PointLight(Tuple.CreatePoint(-10, 10, -10),
        new Color(1, 0.65, 0))
};
// world.Shapes.Add(floor);
// world.Shapes.Add(wall);
// world.Shapes.Add(middle);
// world.Shapes.Add(right);
// world.Shapes.Add(left);
// world.Shapes.Add(cube);
// world.Shapes.Add(cylinder);
world.Shapes.Add(Hexagon(Matrix.Identity.Scale(3, 3, 3)));

var camera = new Camera(1000, 500, Math.PI / 3,
    TransformationFactory.View(Tuple.CreatePoint(0, 5, -10),
        Tuple.CreatePoint(0, 1, 0),
        Tuple.CreatePoint(0, 1, 0)));

Log("Tracing rays...");
var stopWatch = Stopwatch.StartNew();
var canvas = await camera.Render(world);
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
