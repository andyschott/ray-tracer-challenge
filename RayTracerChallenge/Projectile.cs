using Tuple = RayTracerChallenge.Domain.Tuple;

namespace RayTracerChallenge;

public record Projectile(Tuple Position,
    Tuple Velocity);
