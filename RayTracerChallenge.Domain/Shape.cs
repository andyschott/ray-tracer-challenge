namespace RayTracerChallenge.Domain;

public abstract class Shape
{
    protected Shape()
    {
    }

    public Matrix Transform { get; set; } = Matrix.Identity();
    public Material Material { get; set; } = new Material();

    public IEnumerable<Intersection> Intersects(Ray ray)
    {
        ray = ray.Transform(Transform.Invert());
        return LocalIntersects(ray); 
    }

    protected abstract IEnumerable<Intersection> LocalIntersects(Ray ray);

    public Tuple NormalAt(Tuple point)
    {
        if(!point.IsPoint)
        {
            throw new ArgumentException($"{nameof(point)} must be a point");
        }

        var inverseTransform = Transform.Invert();

        var objectPoint = inverseTransform * point;
        var objectNormal = LocalNormalAt(objectPoint);

        var worldNormal = inverseTransform.Transpose() * objectNormal;
        worldNormal = Tuple.CreateVector(worldNormal.X, worldNormal.Y, worldNormal.Z);

        return worldNormal.Normalize();
    }

    protected abstract Tuple LocalNormalAt(Tuple point);
}

