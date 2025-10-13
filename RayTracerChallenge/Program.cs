using System.Diagnostics;
using RayTracerChallenge.Domain;
using RayTracerChallenge.IO;
using Tuple = RayTracerChallenge.Domain.Tuple;

var c = new Canvas(500, 500);

var rayOrigin = Tuple.CreatePoint(0, 0, -5);
var wallZ = 10M;
var wallSize = 7M;
var pixelSize = wallSize / c.Width;
var half = wallSize / 2;

var shape = new Sphere
{
    Material = new Material
    {
        Color = new Color(1, 0.2M, 1)
    }
};

var light = new PointLight(Tuple.CreatePoint(-10, 10, -10),
    new Color(1, 1, 1));

Log("Tracing rays...");
var stopWatch = Stopwatch.StartNew();

await c.Render((x, y) =>
{
    // Compute the world Y coordinate (top = +half, bottom = -half)
    var worldY = half - pixelSize * y;
    
    // Compute the world X coordinate (left = -half, right = half)
    var worldX = -half + pixelSize * x;
        
    // Describe the point on the wall that the ray will target
    var position = Tuple.CreatePoint(worldX, worldY, wallZ);
        
    var r = new Ray(rayOrigin,
        (position - rayOrigin).Normalize());
    var xs = shape.Intersects(r);
    var hit = xs.Hit();
    if (hit is not null)
    {
        var point = r.CalculatePosition(hit.T);
        var normal = hit.Sphere.NormalAt(point);
        var eye = -r.Direction;
            
        var color = hit.Sphere.Material.Lighting(
            light,
            point,
            eye,
            normal);
        return color;
    }

    return new Color(0, 0, 0);
});

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