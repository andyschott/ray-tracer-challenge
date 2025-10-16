namespace RayTracerChallenge.Domain;

public record Intersection(
    double T,
    Shape Shape)
{
    public record Computation(
        double T,
        Shape Shape,
        Tuple Point,
        Tuple OverPoint,
        Tuple UnderPoint,
        Tuple EyeVector,
        Tuple NormalVector,
        Tuple ReflectionVector,
        double N1,
        double N2,
        bool IsInside)
    {
        public double NRatio { get; } = N1 / N2;
        
        public double Schlick()
        {
            var cos = EyeVector.Dot(NormalVector);

            // Total Internal Reflection can only occur if n1 > n2
            if (N1 > N2)
            {
                var sin2_t = Math.Pow(NRatio, 2) * (1.0 - Math.Pow(cos, 2));
                if (sin2_t > 1.0)
                {
                    return 1.0;
                }

                var cos_t = Math.Sqrt(1.0 - sin2_t);

                cos = cos_t;
            }

            var r0 = Math.Pow((N1 - N2) / (N1 + N2), 2);
            return r0 + (1 - r0) * Math.Pow(1 - cos, 5);
        }
    }

    public Computation PrepareComputations(Ray ray, Intersections? xs = null)
    {
        xs ??= [this];
        
        var point = ray.CalculatePosition(T);
        var eyeVector = -ray.Direction;
        var normalVector = Shape.NormalAt(point);

        var inside = false;
        if (normalVector.Dot(eyeVector) < 0)
        {
            inside = true;
            normalVector = -normalVector;
        }
        
        var reflectionVector = ray.Direction.Reflect(normalVector);
        
        var overPoint = point + normalVector * Constants.Epsilon;
        var underPoint = point - normalVector * Constants.Epsilon;

        var (n1, n2) = ComputeRefractions(xs);
        
        return new Computation(T,
            Shape,
            point,
            overPoint,
            underPoint,
            eyeVector,
            normalVector,
            reflectionVector,
            n1,
            n2,
            inside);
    }

    private (double n1, double n2) ComputeRefractions(Intersections xs)
    {
        var containers = new List<Shape>();

        var n1 = 0D;
        var n2 = 0D;

        foreach (var i in xs)
        {
            if (this == i)
            {
                if (containers.Count > 0)
                {
                    n1 = containers.Last().Material.RefractiveIndex;
                }
                else
                {
                    n1 = 1;
                }
            }
            
            var index = containers.IndexOf(i.Shape);
            if (index is not -1)
            {
                containers.RemoveAt(index);
            }
            else
            {
                containers.Add(i.Shape);
            }

            if (this == i)
            {
                if (containers.Count > 0)
                {
                    n2 = containers.Last().Material.RefractiveIndex;
                }
                else
                {
                    n2 = 1;
                }
            }
        }
        
        return (n1, n2);
    }
}