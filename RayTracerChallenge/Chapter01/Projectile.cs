using Tuple = RayTracerChallenge.Domain.Tuple;

namespace RayTracerChallenge.Chapter01;

public record Projectile(Tuple Position,
    Tuple Velocity);
