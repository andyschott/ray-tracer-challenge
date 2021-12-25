namespace RayTracerChallenge.Models;

public class Environment
{
    public Tuple Gravity { get; init; } = Tuple.CreateVector(0, 0, 0);
    public Tuple Wind { get; init; } = Tuple.CreateVector(0, 0, 0);

    public Environment(Tuple gravity, Tuple wind)
    {
        Gravity = gravity;
        Wind = wind;
    }

    public Environment()
    {
    }
}
