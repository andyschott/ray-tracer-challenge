using Tuple = RayTracerChallenge.Domain.Tuple;

namespace RayTracerChallenge.IO;

public interface IModelParser
{
    Model Parse(TextReader reader);
}