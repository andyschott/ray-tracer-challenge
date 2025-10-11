using RayTracerChallenge;
using RayTracerChallenge.Domain;
using RayTracerChallenge.IO;
using Environment = RayTracerChallenge.Environment;
using Tuple = RayTracerChallenge.Domain.Tuple;

// Projectile starts one unit above the origin
// Velocity is normalized to 1 unit per tick
var p = new Projectile(Tuple.CreatePoint(0, 1, 0),
    Tuple.CreateVector(1, 1.8M, 0).Normalize() * 11.25M);
        
// Gravity -0.1 unit per tick
// Wind -0.01 unit per tick
var e = new Environment(Tuple.CreateVector(0, -0.1M, 0),
    Tuple.CreateVector(-0.01M, 0, 0));

var c = new Canvas(900, 550);
var red = new Color(1, 0.69M, 0.69M);

while (p.Position.Y > 0)
{
    WriteProjectile(p.Position);
    p = Tick(e, p);
}

var ppm = new CanvasPpmSerializer()
    .Serialize(c);
using var writer = new StreamWriter("output.ppm");
writer.Write(ppm);

return;

static Projectile Tick(Environment environment,
    Projectile projectile)
{
    var position = projectile.Position + projectile.Velocity;
    var velocity = projectile.Velocity +
                   environment.Gravity + environment.Wind;
        
    return new Projectile(position, velocity);
}

void WriteProjectile(Tuple position)
{
    var x = (int)Math.Floor(position.X);
    var y = (int)Math.Floor(c.Height - position.Y);

    c[x, y] = red;
}
