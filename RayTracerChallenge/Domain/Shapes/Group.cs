using System.Collections;

namespace RayTracerChallenge.Domain.Shapes;

public record Group : Shape, IList<Shape>
{
    private readonly List<Shape> _shapes = [];
    
    public Group(Matrix? transform = null)
        : base(transform)
    {
    }
    
    protected override Intersections CalculateIntersection(Ray ray)
    {
        // This doesn't work :(
        // var bounds = GetBounds();
        // if (!ray.Intersects(bounds))
        // {
        //     return [];
        // }
        
        var intersections = new List<Intersection>();

        foreach (var shape in _shapes)
        {
            var xs = shape.Intersects(ray);
            intersections.AddRange(xs);
        }
        
        intersections.Sort();

        return new Intersections(intersections);
    }

    protected override Tuple CalculateNormal(Tuple objectPoint, Intersection hit)
    {
        throw new NotSupportedException();
    }

    public override Bounds GetBounds()
    {
        var minX = double.MaxValue;
        var minY = double.MaxValue;
        var minZ = double.MaxValue;
        var maxX = double.MinValue;
        var maxY = double.MinValue;
        var maxZ = double.MinValue;
        
        foreach (var shape in _shapes)
        {
            var childBounds = shape.GetBounds();
            var min = shape.Transform * childBounds.Minimum;
            var max = shape.Transform * childBounds.Maximum;
            
            minX = Math.Min(minX, min.X);
            minY = Math.Min(minY, min.Y);
            minZ = Math.Min(minZ, min.Z);
            
            maxX = Math.Max(maxX, max.X);
            maxY = Math.Max(maxY, max.Y);
            maxZ = Math.Max(maxZ, max.Z);
        }
        
        var corner1 = Tuple.CreatePoint(minX, minY, minZ);
        var corner2 = Tuple.CreatePoint(maxX, minY, minZ);
        var corner3 = Tuple.CreatePoint(maxX, maxY, minZ);
        var corner4 = Tuple.CreatePoint(minX, maxY, minZ);

        var corner5 = Tuple.CreatePoint(minX, minY, maxZ);
        var corner6 = Tuple.CreatePoint(maxX, minY, maxZ);
        var corner7 = Tuple.CreatePoint(maxX, maxY, maxZ);
        var corner8 = Tuple.CreatePoint(minX, maxY, maxZ);

        var overallMinX = Math.Min(Math.Min(Math.Min(corner1.X, corner2.X), Math.Min(corner3.X, corner4.X)),
            Math.Min(Math.Min(corner5.X, corner6.X), Math.Min(corner7.X, corner8.X)));
        var overallMinY = Math.Min(Math.Min(Math.Min(corner1.Y, corner2.Y), Math.Min(corner3.Y, corner4.Y)),
            Math.Min(Math.Min(corner5.Y, corner6.Y), Math.Min(corner7.Y, corner8.Y)));
        var overallMinZ = Math.Min(Math.Min(Math.Min(corner1.Z, corner2.Z), Math.Min(corner3.Z, corner4.Z)),
            Math.Min(Math.Min(corner5.Z, corner6.Z), Math.Min(corner7.Z, corner8.Z)));

        var overallMaxX = Math.Max(Math.Max(Math.Max(corner1.X, corner2.X), Math.Max(corner3.X, corner4.X)),
            Math.Max(Math.Max(corner5.X, corner6.X), Math.Max(corner7.X, corner8.X)));
        var overallMaxY = Math.Max(Math.Max(Math.Max(corner1.Y, corner2.Y), Math.Max(corner3.Y, corner4.Y)),
            Math.Max(Math.Max(corner5.Y, corner6.Y), Math.Max(corner7.Y, corner8.Y)));
        var overallMaxZ = Math.Max(Math.Max(Math.Max(corner1.Z, corner2.Z), Math.Max(corner3.Z, corner4.Z)),
            Math.Max(Math.Max(corner5.Z, corner6.Z), Math.Max(corner7.Z, corner8.Z)));

        return new Bounds(overallMinX, overallMinY, overallMinZ,
            overallMaxX, overallMaxY, overallMaxZ);
    }

    public IEnumerator<Shape> GetEnumerator() => _shapes.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Add(Shape item)
    {
        _shapes.Add(item);
        item.Parent = this;
    }

    public void Clear()
    {
        foreach (var item in _shapes)
        {
            item.Parent = null;
        }
        _shapes.Clear();
    }

    public bool Contains(Shape item) => _shapes.Contains(item);

    public void CopyTo(Shape[] array, int arrayIndex)
    {
        throw new NotSupportedException();
    }

    public bool Remove(Shape item)
    {
        item.Parent = null;
        return _shapes.Remove(item);
    }

    public int Count => _shapes.Count;
    public bool IsReadOnly => false;
    public int IndexOf(Shape item) => _shapes.IndexOf(item);

    public void Insert(int index, Shape item)
    {
        _shapes.Insert(index, item);
        item.Parent = this;
    }

    public void RemoveAt(int index)
    {
        _shapes[index].Parent = null;
        _shapes.RemoveAt(index);
    }

    public Shape this[int index]
    {
        get => _shapes[index];
        set
        {
            _shapes[index] = value;
            value.Parent = this;
        }
    }
}