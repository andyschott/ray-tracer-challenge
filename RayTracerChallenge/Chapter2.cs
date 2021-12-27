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
        var start = Tuple.CreatePoint(0, 1, 1);
        var velocity = Tuple.CreateVector(1, 1.8M, 0).Normalize() * 11.25M;
        var projectile = new Projectile(start, velocity);

        var gravity = Tuple.CreateVector(0, -0.1M, 0);
        var wind = Tuple.CreateVector(-0.01M, 0, 0);
        var environment = new Environment(gravity, wind);

        var canvas = new Canvas(900, 550);

        while(projectile.Position.Y > 0)
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
