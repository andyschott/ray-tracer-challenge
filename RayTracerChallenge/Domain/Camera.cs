using System.Collections.Concurrent;

namespace RayTracerChallenge.Domain;

public record Camera
{
    public int HorizontalSize { get; init; }
    public int VerticalSize { get; init; }
    public double FieldOfView { get; init; }
    public Matrix Transform { get; init; }
    
    public double PixelSize { get; }

    private readonly double _halfWidth;
    private readonly double _halfHeight;
    private readonly Matrix _inverseTransform;
    private readonly Tuple _origin;

    public Camera(int horizontalSize,
        int verticalSize,
        double fieldOfView,
        Matrix? transform = null)
    {
        HorizontalSize = horizontalSize;
        VerticalSize = verticalSize;
        FieldOfView = fieldOfView;
        Transform = transform ?? Matrix.Identity;
        _inverseTransform = Transform.Inverse();
        _origin = _inverseTransform * Tuple.CreatePoint(0, 0, 0);

        var halfView = Math.Tan(FieldOfView / 2);
        var aspect = (double)HorizontalSize / VerticalSize;
        if (aspect >= 1)
        {
            _halfWidth = halfView;
            _halfHeight = halfView / aspect;
        }
        else
        {
            _halfWidth = halfView * aspect;
            _halfHeight = halfView;
        }
        
        PixelSize = (_halfWidth * 2) / HorizontalSize;
    }

    public Ray RayForPixel(int x, int y)
    {
        // Offset from the edge of the canvas to the pixel's center
        var xOffset = (x + 0.5) * PixelSize;
        var yOffset = (y + 0.5) * PixelSize;
        
        // Untransformed Coordinates of the pixel in world space
        // (Camera looks towards +Z, so +x is to the left)
        var worldX = _halfWidth - xOffset;
        var worldY = _halfHeight - yOffset;
        
        // Using the camera matrix, transform the camera point and origin,
        // and then compute the ray's direction vector
        // (canvas is at Z = -1)
        var pixel = _inverseTransform * Tuple.CreatePoint(worldX, worldY, -1);
        var direction = (pixel - _origin).Normalize();

        return new Ray(_origin, direction);
    }

    public Canvas Render(World world)
    {
        var image = new Canvas(HorizontalSize, VerticalSize);

        var logicalProcessors = Environment.ProcessorCount;
        var pairs = Enumerable.Range(0, VerticalSize)
            .SelectMany(y =>
            {
                return Enumerable.Range(0, HorizontalSize).Select(x => (x, y));
            });

        var workQueue = new ConcurrentQueue<(int x, int y)>();
        foreach (var item in pairs)
        {
            workQueue.Enqueue(item);
        }
        
        var tp = new ThreadParams(workQueue,
            this,
            image,
            world);
        var threads = new Thread[logicalProcessors];
        for (var i = 0; i < logicalProcessors; i++)
        {
            var t = new Thread(RenderThread);
            t.Start(tp);

            threads[i] = t;
        }

        foreach (var t in threads)
        {
            t.Join();
        }
        
        return image;
    }

    private static void RenderThread(object? data)
    {
        var (workQueue, camera, canvas, world) = (ThreadParams)data!;
        while (workQueue.TryDequeue(out var pair))
        {
            var ray = camera.RayForPixel(pair.x, pair.y);
            var color = world.ColorAt(ray);
            canvas[pair.x, pair.y] = color;
        }
    }

    private record ThreadParams(ConcurrentQueue<(int x, int y)> WorkQueue,
        Camera Camera,
        Canvas Canvas,
        World World);
}