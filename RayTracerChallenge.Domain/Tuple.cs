namespace RayTracerChallenge.Domain;
public class Tuple
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
    public float W { get; set; }

    public bool IsPoint => W == Point;
    public bool IsVector => W == Vector;

    private const float Point = 1.0F;
    private const float Vector = 0.0F;

    public static Tuple CreatePoint(float x, float y, float z)
    {
        return new Tuple
        {
            X = x,
            Y = y,
            Z = z,
            W = Point
        };
    }

    public static Tuple CreateVector(float x, float y, float z)
    {
        return new Tuple
        {
            X = x,
            Y = y,
            Z = z,
            W = Vector
        };
    }
}
