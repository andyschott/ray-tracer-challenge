using Tuple = RayTracerChallenge.Domain.Tuple;

namespace RayTracerChallenge;

public record Environment(Tuple Gravity,
    Tuple Wind);