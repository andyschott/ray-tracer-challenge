using System.Diagnostics;
using RayTracerChallenge.Domain;
using RayTracerChallenge.Extensions;
using RayTracerChallenge.IO;
using Tuple = RayTracerChallenge.Domain.Tuple;

var floor = new Plane
{
    Material = new Material
    {
        Color = new Color(0.2, 0.41, 0),
        Specular = 0,
        Pattern = new CheckersPattern(new Color(0.2, 0.41, 0),
            new Color(1, 1, 1))
    }
};

var wall = new Plane(Matrix.Identity.RotateX(Math.PI / 2)
    .Translate(0, 0, 5))
{
    Material = new Material
    {
        Color = new Color(0.5, 1, 0),
        Specular = 0,
        Pattern = new CheckersPattern(new Color(0.5, 1, 0),
            new Color(1, 1, 0))
    }
};

var middle = new Sphere(Matrix.Identity.Translate(-0.5, 1, 0.5))
{
    Material = new Material
    {
        Color = new Color(1, 0, 1),
        Diffuse = 0.7,
        Specular = 0.3,
        // Pattern = new GradientPattern(new Color(0, 0, 1),
        //     new Color(1, 0, 1))
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

var world = new World
{
    Light = new PointLight(Tuple.CreatePoint(-10, 10, -10),
        new Color(1, 1, 1))
};
world.Shapes.Add(floor);
world.Shapes.Add(wall);
world.Shapes.Add(middle);
world.Shapes.Add(right);
world.Shapes.Add(left);

var camera = new Camera(1000, 500, Math.PI / 3,
    TransformationFactory.View(Tuple.CreatePoint(0, 1.5, -5),
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