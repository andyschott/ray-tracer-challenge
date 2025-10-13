using System.Diagnostics;
using RayTracerChallenge.Domain;
using RayTracerChallenge.IO;
using Tuple = RayTracerChallenge.Domain.Tuple;

var c = new Canvas(500, 500);
var red = new Color(1, 0, 0);

var rayOrigin = Tuple.CreatePoint(0, 0, -5);
var wallZ = 10M;
var wallSize = 7M;
var pixelSize = wallSize / c.Width;
var half = wallSize / 2;

var shape = new Sphere();

Log("Tracing rays...");
var stopWatch = Stopwatch.StartNew();

for (var y = 0; y < c.Height; y++)
{
    // Compute the world Y coordinate (top = +half, bottom = -half)
    var worldY = half - pixelSize * y;

    for (var x = 0; x < c.Width; x++)
    {
        // Compute the world X coordinate (left = -half, right = half)
        var worldX = -half + pixelSize * x;
        
        // Describe the point on the wall that the ray will target
        var position = Tuple.CreatePoint(worldX, worldY, wallZ);
        
        var r = new Ray(rayOrigin,
            (position - rayOrigin).Normalize());
        var xs = shape.Intersects(r);
        if (xs.Hit() is not null)
        {
            c[x, y] = red;
        }
    }
}
stopWatch.Stop();
Log($"Done. Took {stopWatch.ElapsedMilliseconds}ms");

Log("Saving canvas to disk...");
using var writer = new StreamWriter("output.ppm");
new CanvasPpmSerializer()
    .Serialize(c, writer);
Log("Done.");

return;

static void Log(string str)
{
    Console.WriteLine($"[{DateTime.Now}] {str}");
}