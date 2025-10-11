using RayTracerChallenge.Domain;

namespace RayTracerChallenge.IO;

public interface ICanvasSerializer
{
    string Serialize(Canvas canvas);
}