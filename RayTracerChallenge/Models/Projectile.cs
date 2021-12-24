namespace RayTracerChallenge.Models;

public class Projectile
{
    public Tuple Position { get; init; } = Tuple.CreatePoint(0.0F, 0.0F, 0.0F);
    public Tuple Velocity { get; init; } = Tuple.CreateVector(0.0F, 0.0F, 0.0F);

    public Projectile(Tuple position, Tuple velocity)
    {
        Position = position;
        Velocity = velocity;
    }

    public Projectile()
    {
    }
}
