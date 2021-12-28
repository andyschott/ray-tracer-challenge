namespace RayTracerChallenge.Domain;

public class Camera
{
    public int Width { get; init; }
    public int Height { get; init; }
    public double FieldOfView { get; init; }
    public Matrix Transform { get; init; }

    public decimal PixelSize { get; init; }
    public decimal HalfWidth { get; init; }
    public decimal HalfHeight { get; init; }

    public Camera(int width, int height, double fieldOfView, Matrix? transform = null)
    {
        Width = width;
        Height = height;
        FieldOfView = fieldOfView;
        Transform = transform ?? Matrix.Identity();

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
        var inverseCameraTransform = Transform.Invert();
        var pixel = inverseCameraTransform * Tuple.CreatePoint(worldX, worldY, -1);
        var origin = inverseCameraTransform * Tuple.CreatePoint(0, 0, 0);
        var direction = (pixel - origin).Normalize();

        return new Ray(origin, direction);
    }

    public Canvas Render(World world)
    {
        var canvas = new Canvas(Width, Height);

        for(var y = 0; y < Height; ++y)
        {
            for(var x = 0; x < Width; ++x)
            {
                var ray = RayForPixel(x, y);
                var color = world.ColorAt(ray);
                canvas[x, y] = color;
            }
        }

        return canvas;
    }
}
