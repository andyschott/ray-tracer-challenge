using System.Diagnostics;
using RayTracerChallenge.Domain;
using RayTracerChallenge.Extensions;
using RayTracerChallenge.IO;
using Tuple = RayTracerChallenge.Domain.Tuple;

var floor = new Sphere(Matrix.Identity.Scale(10, 0.01, 10))
{
    Material = new Material
    {
        Color = new Color(1, 0.9, 0.9),
        Specular = 0,
    }
};

var leftWall = new Sphere(Matrix.Identity.Scale(10, 0.01, 10)
    .RotateX(Math.PI / 2)
    .RotateY(-Math.PI / 4)
    .Translate(0, 0, 5))
{
    Material = floor.Material
};

var rightWall = new Sphere(Matrix.Identity.Scale(10, 0.01, 10)
    .RotateX(Math.PI / 2)
    .RotateY(Math.PI / 4)
    .Translate(0, 0, 5))
{
    Material = floor.Material
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

var world = new World
{
    Light = new PointLight(Tuple.CreatePoint(-10, 10, -10),
        new Color(1, 1, 1))
};
world.Objects.Add(floor);
world.Objects.Add(leftWall);
world.Objects.Add(rightWall);
world.Objects.Add(middle);
world.Objects.Add(right);
world.Objects.Add(left);

var camera = new Camera(2000, 1000, Math.PI / 3,
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