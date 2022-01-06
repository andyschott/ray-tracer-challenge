using System.Threading.Tasks;
using RayTracerChallenge.Domain.Patterns;

namespace RayTracerChallenge;

public static class Chapter10
{
    private static readonly TransformationFactory _factory = new TransformationFactory();

    public static async Task<string> CreateWorld()
    {
        var world = new World
        {
            Objects = CreateObjects(),
            Light = new Light(Tuple.CreatePoint(-10, 10, -10), Color.White)
        };

        var camera = new Camera(1000, 500, Math.PI / 3,
            _factory.TransformView(Tuple.CreatePoint(0, 1.5M, -5),
                Tuple.CreatePoint(0, 1, 0),
                Tuple.CreateVector(0, 1, 0)));
        
        var canvas = await camera.RenderAsync(world);

        var writer = new PPMWriter();
        return writer.Save(canvas);
    }

    private static IList<Shape> CreateObjects()
    {
        var wallMaterial = new Material
        {
            Pattern = new SolidPattern(new Color
            {
                Red = 1,
                Green = 0.9M,
                Blue = 0.9M
            }),
            Specular = 0
        };

        var floor = new Plane
        {
            Transform = _factory.RotationAroundYAxis(Math.PI / 4),
            Material = new Material
            {
                Pattern = new CheckerPattern(new Color
                {
                    Red = 0.75M,
                    Green = 0.75M,
                    Blue = 0.75M
                }, new Color
                {
                    Red = 0.25M,
                    Green = 0.25M,
                    Blue = 0.25M
                }),
                Specular = 0,
                Reflective = 1,
            }
        };

        var leftWall = new Plane
        {
            Transform = _factory.Translation(0, 0, 5) *
                _factory.RotationAroundYAxis(-1 * Math.PI / 4) *
                _factory.RotationAroundXAxis(Math.PI / 2) *
                _factory.Scale(10, 0.01M, 10),
            Material = wallMaterial
        };

        var rightWall = new Plane
        {
            Transform = _factory.Translation(0, 0, 5) *
                _factory.RotationAroundYAxis(Math.PI / 4) *
                _factory.RotationAroundXAxis(Math.PI / 2) *
                _factory.Scale(10, 0.01M, 10),
            Material = wallMaterial
        };

        var middleSphere = new Sphere
        {
            Transform = _factory.Translation(-0.5M, 1, 0.5M),
            Material = new Material
            {
                Pattern = new SolidPattern(new Color
                {
                    Red = 0.1M,
                    Green = 1,
                    Blue = 0.5M
                }),
                Diffuse = 0.7M,
                Specular = 0.3M
            }
        };

        var rightSphere = new Sphere
        {
            Transform = _factory.Translation(1.5M, 0.5M, -0.5M) *
                _factory.Scale(0.5M, 0.5M, 0.5M),
            Material = new Material
            {
                Pattern = new SolidPattern(new Color
                {
                    Red = 0.5M,
                    Green = 1,
                    Blue = 0.1M
                }),
                Diffuse = 0.7M,
                Specular = 0.3M
            }
        };

        var leftSphere = new Sphere
        {
            Transform = _factory.Translation(-1.5M, 0.33M, -0.75M) *
                _factory.Scale(0.33M, 0.33M, 0.33M),
            Material = new Material
            {
                Pattern = new SolidPattern(new Color
                {
                    Red = 1,
                    Green = 0.8M,
                    Blue = 0.1M
                }),
                Diffuse = 0.7M,
                Specular = 0.3M
            }
        };

        return new List<Shape>
        {
            floor,
            leftWall,
            rightWall,
            leftSphere,
            middleSphere,
            rightSphere
        };
    }
}
