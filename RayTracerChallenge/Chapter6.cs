namespace RayTracerChallenge;

public static class Chapter6
{
    public static string SphereShadow()
    {
        var rayOrigin = Tuple.CreatePoint(0, 0, -5);
        var wallZ = 10M;
        var wallSize = 7.0M;
        
        var canvas = new Canvas(100, 100);
        var pixelSize = wallSize / canvas.Width;

        var wallX = wallSize / 2;
        var wallY = wallSize / 2;

        var sphere = new Sphere()
        {
            Material = new Material
            {
                Color = new Color
                {
                    Red = 1,
                    Green = 0.2M,
                    Blue = 1
                }
            }
        };

        var light = new Light(Tuple.CreatePoint(-10, 10, -10), new Color
        {
            Red = 1,
            Green = 1,
            Blue = 1
        });

        for(var y = 0; y < canvas.Height; ++y)
        {
            var worldY = wallY - pixelSize * y;

            for(var x = 0; x < canvas.Width; ++x)
            {
                var worldX = -wallX + pixelSize * x;
                var rayDestination = Tuple.CreatePoint(worldX, worldY, wallZ);

                var ray = new Ray(rayOrigin, (rayDestination - rayOrigin).Normalize());
                var intersections = sphere.Intersects(ray);

                var hit = intersections.Hit();
                if(hit is not null)
                {
                    var point = ray.Position(hit.T);
                    var normal = hit.Object.NormalAt(point);
                    var eye = ray.Direction;

                    var color = hit.Object.Material.Lighting(point, light, eye, normal);
                    canvas.WritePixel(x, y, color);
                }
            }
        }

        var writer = new PPMWriter();
        return writer.Save(canvas);
    } 
}
