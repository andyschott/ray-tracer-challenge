using RayTracerChallenge.Domain;

namespace RayTracerChallenge.IO;

public interface ICanvasSerializer
{
    void Serialize(Canvas canvas, TextWriter writer);
}