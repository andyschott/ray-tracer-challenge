namespace RayTracerChallenge.Domain;

public class Light
{
    public Tuple Position { get; init; }
    public Color Intensity { get; init; }

    public Light(Tuple position, Color intensity)
    {
        if(!position.IsPoint)
        {
            throw new ArgumentException($"{nameof(position)} must be a point");
        }

        Position = position;
        Intensity = intensity;
    }

    public override string ToString() => $"{Intensity} at {Position}";
}
