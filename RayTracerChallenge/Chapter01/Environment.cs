using Tuple = RayTracerChallenge.Domain.Tuple;

namespace RayTracerChallenge.Chapter01;

public record Environment(Tuple Gravity,
    Tuple Wind);