using RayTracerChallenge.Domain;
using RayTracerChallenge.Extensions;
using RayTracerChallenge.IO;
using Tuple = RayTracerChallenge.Domain.Tuple;

var c = new Canvas(500, 500);
var white = new Color(1, 1, 1);

var clockRadius = c.Width * 3 / 8;
var translate = Matrix.Identity
    .Scale(clockRadius, 0, clockRadius)
    .Translate(250, 0, 250);

var twelve = Tuple.CreatePoint(0, 0, 1);
for (var i = 1; i < 12; i++)
{
    var r = translate * Matrix.Identity
        .RotateY(i * Math.PI / 6);
    var point = r * twelve;
    c.WritePixel((int)point.X, (int)point.Z, white);
}

twelve = translate * twelve;
c.WritePixel((int)twelve.X, (int)twelve.Z, white);

var ppm = new CanvasPpmSerializer()
    .Serialize(c);
using var writer = new StreamWriter("output.ppm");
writer.Write(ppm);

