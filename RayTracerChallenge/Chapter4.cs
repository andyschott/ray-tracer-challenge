using System.Collections.Generic;
using System.Linq;
using RayTracerChallenge.IO.PPM;
using Math = System.Math;

namespace RayTracerChallenge;

public static class Chapter4
{
    private static readonly TransformationFactory _factory = new TransformationFactory();

    public static string CreateClock()
    {
        var canvas = new Canvas(200, 200);

        var hours = CreateHourPoints(3.0M / 8.0M * canvas.Width, Tuple.CreateVector(100, 0, 100));

        var red = new Color
        {
            Red = 255,
            Green = 0,
            Blue = 0
        };
        foreach(var point in hours)
        {
            canvas.WritePixel((int)Math.Round(point.X), (int)Math.Round(point.Z), red);
        }

        var writer = new PPMWriter();
        return writer.Save(canvas);
    }

    private static IEnumerable<Tuple> CreateHourPoints(decimal radius, Tuple middle)
    {
        var twelve = Tuple.CreatePoint(0, 0, 1);
        var hours = new Tuple[12];
        hours[0] = twelve;

        for(var index = 1; index < hours.Length; ++index)
        {
            var point = _factory.RotationAroundYAxis(index * Math.PI / 6) * twelve;
            point = Tuple.CreatePoint(point.X * radius, 0, point.Z * radius);
            point = point + middle;

            hours[index] = point;
        }

        return hours;
    }
}
