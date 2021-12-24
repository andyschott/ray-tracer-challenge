namespace RayTracerChallenge.Models;

public class Environment
{
    public Tuple Gravity { get; init; } = Tuple.CreateVector(0.0F, 0.0F, 0.0F);
    public Tuple Wind { get; init; } = Tuple.CreateVector(0.0F, 0.0F, 0.0F);

    public Environment(Tuple gravity, Tuple wind)
    {
        Gravity = gravity;
        Wind = wind;
    }

    public Environment()
    {
    }
}
