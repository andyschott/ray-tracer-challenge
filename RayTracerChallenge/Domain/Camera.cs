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

    public async Task<Canvas> Render(World world)
    {
        var image = new Canvas(HorizontalSize, VerticalSize);

        var logicalProcessors = Environment.ProcessorCount;
        var pairs = Enumerable.Range(0, VerticalSize)
            .SelectMany(y =>
            {
                var something = Enumerable.Range(0, HorizontalSize);
                return something.Select(x => (x, y));
            }).ToArray();
        var chunks = pairs.Chunk(logicalProcessors);
        foreach (var chunk in chunks)
        {
            var tasks = chunk.Select(pair =>
            {
                return Task.Run(() =>
                {
                    var ray = RayForPixel(pair.x, pair.y);
                    var color = world.ColorAt(ray);
                    return (pair.x, pair.y, color);
                });
            }).ToArray();
            
            var results = await Task.WhenAll(tasks);
            foreach (var (x, y, color) in results)
            {
                image[x, y] = color;
            }
        }
        
        return image;
    }
}