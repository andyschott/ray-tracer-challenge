namespace RayTracerChallenge.Domain.Shapes;

public record Csg : Shape
{
    public CsgOperation Operation { get; }
    public Shape Left { get; }
    public Shape Right { get; }

    public Csg(CsgOperation operation,
        Shape left,
        Shape right)
    {
        Operation = operation;
        
        Left = left;
        Left.Parent = this;
        
        Right = right;
        Right.Parent = this;
    }

    public bool IntersectionAllowed(bool leftHit,
        bool isHitInsideLeft,
        bool isHitInsideRight)
    {
        if (Operation is CsgOperation.Union)
        {
            return (leftHit && !isHitInsideRight) ||
                   (!leftHit && !isHitInsideLeft);
        }

        if (Operation is CsgOperation.Intersection)
        {
            return (leftHit && isHitInsideRight) ||
                   (!leftHit && isHitInsideLeft);
        }

        if (Operation is not CsgOperation.Difference)
        {
            throw new ArgumentException(nameof(Operation));
        }

        return (leftHit && !isHitInsideRight) ||
               (!leftHit && isHitInsideLeft);
    }

    public Intersections FilterIntersections(Intersections xs)
    {
        var isHitInsideLeft = false;
        var isHitInsideRight = false;

        var result = new Intersections();

        foreach (var i in xs)
        {
            var isLeftHit = Includes(Left, i.Shape);
            if (IntersectionAllowed(isLeftHit, isHitInsideLeft, isHitInsideRight))
            {
                result.Add(i);
            }

            if (isLeftHit)
            {
                isHitInsideLeft = !isHitInsideLeft;
            }
            else
            {
                isHitInsideRight = !isHitInsideRight;
            }
        }

        return result;
    }

    private static bool Includes(Shape a, Shape b)
    {
        return a switch
        {
            Group g => g.Any(child => Includes(child, b)),
            Csg csg => Includes(csg.Left, b) || Includes(csg.Right, b),
            _ => a == b
        };
    }

    public override Bounds GetBounds()
    {
        throw new NotImplementedException();
    }

    protected override Intersections CalculateIntersection(Ray ray)
    {
        var intersections = new List<Intersection>();
        intersections.AddRange(Left.Intersects(ray));
        intersections.AddRange(Right.Intersects(ray));
        intersections.Sort();

        return FilterIntersections(new Intersections(intersections));
    }

    protected override Tuple CalculateNormal(Tuple objectPoint, Intersection hit)
    {
        throw new NotImplementedException();
    }
}