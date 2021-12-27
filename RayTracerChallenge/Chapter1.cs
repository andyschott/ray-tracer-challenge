namespace RayTracerChallenge;

public class Chapter1
{
    protected static Projectile Tick(Environment environment, Projectile projectile)
    {
        var position = projectile.Position + projectile.Velocity;
        var velocity = projectile.Velocity + environment.Gravity + environment.Wind;

        if(position.Y < 0)
        {
            position = Tuple.CreatePoint(position.X, 0, position.Z);
        }

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
            Position = Tuple.CreatePoint(0, 1, 0),
            Velocity = Tuple.CreateVector(1, 1, 0).Normalize()
        };

        // Gravity is -0.1 unit/tick, and wind is -0.01 unit/tick
        var environment = new Environment
        {
            Gravity = Tuple.CreateVector(0, -0.1M, 0),
            Wind = Tuple.CreateVector(-0.01M, 0, 0)
        };

        var iterations = 0;
        while(projectile.Position.Y > 0)
        {
            Console.WriteLine($"Projectile is at {projectile.Position.Y} units above the origin");
            projectile = Tick(environment, projectile);

            ++iterations;
        }

        Console.WriteLine($"Took {iterations} for the projectile to reach the origin");
    }
}
