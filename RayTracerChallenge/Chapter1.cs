using RayTracerChallenge.Domain;
using Console = System.Console;

namespace RayTracerChallenge;
public class Chapter1
{
    class Projectile
    {
        public Tuple Position { get; init; } = Tuple.CreatePoint(0.0F, 0.0F, 0.0F);
        public Tuple Velocity { get; init; } = Tuple.CreateVector(0.0F, 0.0F, 0.0F);
    }

    class Environment
    {
        public Tuple Gravity { get; init; } = Tuple.CreateVector(0.0F, 0.0F, 0.0F);
        public Tuple Wind { get; init; } = Tuple.CreateVector(0.0F, 0.0F, 0.0F);
    }

    private static Projectile Tick(Environment environment, Projectile projectile)
    {
        var position = projectile.Position + projectile.Velocity;
        var velocity = projectile.Velocity + environment.Gravity + environment.Wind;

        return new Projectile
        {
            Position = position,
            Velocity = velocity
        };
    }

    public void SimulateEnvironment()
    {
        // Projectile starts one unit above the origin
        // Velocity is normalized to 1 unit/tick
        var projectile = new Projectile
        {
            Position = Tuple.CreatePoint(0.0F, 1.0F, 0.0F),
            Velocity = Tuple.CreateVector(1.0F, 1.0F, 0.0F).Normalize()
        };

        // Gravity is -0.1 unit/tick, and wind is -0.01 unit/tick
        var environment = new Environment
        {
            Gravity = Tuple.CreateVector(0.0F, -0.1F, 0.0F),
            Wind = Tuple.CreateVector(-0.01F, 0.0F, 0.0F)
        };

        var iterations = 0;
        while(projectile.Position.Y > 0.0F)
        {
            Console.WriteLine($"Projectile is at {projectile.Position.Y} units above the origin");
            projectile = Tick(environment, projectile);

            ++iterations;
        }

        Console.WriteLine($"Took {iterations} for the projectile to reach the origin");
    }
}