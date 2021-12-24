using System.Collections.Generic;
using System.Linq;
using RayTracerChallenge.IO.PPM;
using RayTracerChallenge.Models;

namespace RayTracerChallenge;

public class Chapter2 : Chapter1
{
    private static readonly Color _pixelColor = new Color
    {
        Red = 255,
        Green = 0,
        Blue = 0
    };

    public string PlotProjectileTragectory()
    {
        var start = Tuple.CreatePoint(0.0F, 1.0F, 1.0F);
        var velocity = Tuple.CreateVector(1.0F, 1.8F, 0.0F).Normalize() * 11.25F;
        var projectile = new Projectile(start, velocity);

        var gravity = Tuple.CreateVector(0.0F, -0.1F, 0.0F);
        var wind = Tuple.CreateVector(-0.01F, 0.0F, 0.0F);
        var environment = new Environment(gravity, wind);

        var canvas = new Canvas(900, 550);

        while(projectile.Position.Y > 0.0F)
        {
            projectile = Tick(environment, projectile);
            WriteToCanvas(projectile.Position, canvas);
        }

        var writer = new PPMWriter();
        return writer.Save(canvas);
    }

    private void WriteToCanvas(Tuple position, Canvas canvas)
    {
        var x = (int)position.X;
        var y = (int)(canvas.Height - 1 - position.Y);

        canvas.WritePixel(x, y, _pixelColor);
    }
}
