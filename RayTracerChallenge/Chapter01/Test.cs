using Tuple = RayTracerChallenge.Domain.Tuple;

namespace RayTracerChallenge.Chapter01;

public static class Test
{
    public static void Run()
    {
        // Projectile starts one unit above the origin
        // Velocity is normalized to 1 unit per tick
        var p = new Projectile(Tuple.CreatePoint(0, 1, 0),
            Tuple.CreateVector(1, 1, 0).Normalize());
        
        // Gravity -0.1 unit per tick
        // Wind -0.01 unit per tick
        var e = new Environment(Tuple.CreateVector(0, -0.1M, 0),
            Tuple.CreateVector(-0.01M, 0, 0));

        for (var tick = 1; p.Position.Y > 0; tick++)
        {
            p = Tick(e, p);
            Console.WriteLine($"{tick}. {p.Position}");
        }
    }

    private static Projectile Tick(Environment environment,
        Projectile projectile)
    {
        var position = projectile.Position + projectile.Velocity;
        var velocity = projectile.Velocity +
                       environment.Gravity + environment.Wind;
        
        return new Projectile(position, velocity);
    }
}