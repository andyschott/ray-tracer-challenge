namespace RayTracerChallenge.Domain;

public class Camera
{
    public int Width { get; }
    public int Height { get; }
    public double FieldOfView { get; }
    public Matrix Transform { get; }

    public decimal PixelSize { get; }
    public decimal HalfWidth { get; }
    public decimal HalfHeight { get; }

    private readonly Matrix _inverseTransform;
    private readonly Tuple _origin;

    public Camera(int width, int height, double fieldOfView, Matrix? transform = null)
    {
        Width = width;
        Height = height;
        FieldOfView = fieldOfView;
        Transform = transform ?? Matrix.Identity();

        _inverseTransform = Transform.Invert();
        _origin = _inverseTransform * Tuple.CreatePoint(0, 0, 0);

        (HalfWidth, HalfHeight, PixelSize) = ComputeDimensions(width, height, fieldOfView);
    }

    private static (decimal halfWidth, decimal halfHeight, decimal pixelSize) ComputeDimensions(int width, int height, double fieldOfView)
    {
        var halfView = Math.Tan(fieldOfView / 2);
        var aspectRatio = (double)width / (double)height;

        double halfWidth;
        double halfHeight;

        if(aspectRatio >= 1)
        {
            halfWidth = halfView;
            halfHeight = halfView / aspectRatio;
        }
        else
        {
            halfWidth = halfView * aspectRatio;
            halfHeight = halfView;
        }

        return (Convert.ToDecimal(halfWidth),
            Convert.ToDecimal(halfHeight),
            Convert.ToDecimal((halfWidth * 2) / width));
    }

    public Ray RayForPixel(int x, int y)
    {
        // the offset from the edge of the canvas to the pixel's center
        var xOffset = (x + 0.5M) * PixelSize;
        var yOffset = (y + 0.5M) * PixelSize;

        // the untransformed coordinates of the pixel in world space
        var worldX = HalfWidth - xOffset;
        var worldY = HalfHeight - yOffset;

        // using the camera matrix, transform the canvas point and the origin,
        // and then compute the ray's direction vector
        // (this assumes the camera is at z = -1)
        var pixel = _inverseTransform * Tuple.CreatePoint(worldX, worldY, -1);
        var direction = (pixel - _origin).Normalize();

        return new Ray(_origin, direction);
    }

    public Canvas Render(World world)
    {
        var canvas = new Canvas(Width, Height);

        for(var y = 0; y < Height; ++y)
        {
            for(var x = 0; x < Width; ++x)
            {
                var color = ColorAt(world, x, y);
                canvas[x, y] = color;
            }
        }

        return canvas;
    }

    public async Task<Canvas> RenderAsync(World world)
    {
        var pixels = GetPixels(Width, Height);
        var processors = System.Environment.ProcessorCount;

        var canvas = new Canvas(Width, Height);

        for(var index = 0; index < pixels.Length; index += processors)
        {
            var chunk = pixels.Skip(index).Take(processors);
            var tasks = chunk.Select(item => Task.Run(() =>
            {
                var color = ColorAt(world, item.x, item.y);
                return (item.x, item.y, color);
            }));

            var results = await Task.WhenAll(tasks);
            foreach(var item in results)
            {
                canvas[item.x, item.y] = item.color;
            }
        }

        return canvas;
    }

    private static (int x, int y)[] GetPixels(int width, int height)
    {
        var pixels = new (int x, int y)[width * height];

        var index = 0;
        for(var y = 0; y < height; ++y)
        {
            for(var x = 0; x < width; ++x)
            {
                pixels[index++] = (x, y);
            }
        }

        return pixels;
    }

    private Color ColorAt(World world, int x, int y)
    {
        var ray = RayForPixel(x, y);
        return world.ColorAt(ray);
    }
}
