namespace RayTracerChallenge;

public static class Chapter5
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

        var red = new Color
        {
            Red = 255,
            Green = 0,
            Blue = 0
        };

        var sphere = new Sphere();

        for(var y = 0; y < canvas.Height; ++y)
        {
            var worldY = wallY - pixelSize * y;

            for(var x = 0; x < canvas.Width; ++x)
            {
                var worldX = -wallX + pixelSize * x;
                var rayDestination = Tuple.CreatePoint(worldX, worldY, wallZ);

                var ray = new Ray(rayOrigin, (rayDestination - rayOrigin).Normalize());
                var intersections = sphere.Intersects(ray);

                if(intersections.Hit() is not null)
                {
                    canvas.WritePixel(x, y, red);
                }
            }
        }

        var writer = new PPMWriter();
        return writer.Save(canvas);
    } 
}
