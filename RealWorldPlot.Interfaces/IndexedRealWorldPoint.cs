namespace RealWorldPlot.Interfaces;

/// <summary>
/// Point du monde réel, avec un identifiant unique et des coordonnées X et Y.
/// </summary>
/// <param name="id"></param>
/// <param name="x"></param>
/// <param name="y"></param>
public class IndexedRealWorldPoint : RealWorldPoint
{
    private readonly RealWorldPoints _owner;

    internal IndexedRealWorldPoint(RealWorldPoints owner,Guid id, double x, double y) : base(x, y)
    {
        _owner = owner;
        Id = id;
    }


    public Guid Id { get; set; }

    public bool Equals(IndexedRealWorldPoint other)
    {
        return Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        return obj is IndexedRealWorldPoint other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
    public void SetFirst()
    {
        PreviousPoint = null;
    }
    internal void setNext(IndexedRealWorldPoint newPoint)
    {
        this.NextPoint = newPoint;
        newPoint.PreviousPoint = this;
    }

    public IndexedRealWorldPoint PreviousPoint { get; private set; }

    public IndexedRealWorldPoint NextPoint { get; private set; }

    public void UpdateFrom(RealWorldPoint newReal)
    {
        X = newReal.X;
        Y = newReal.Y;
    }

    public override string ToString()
    {
        return $"Point {Id} at ({X}, {Y})";
    }

    public double DistanceTo(IndexedRealWorldPoint other)
    {
        return Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2));
    }

    public void EndUpdate()
    {
        _owner.dataChanged();
    }
}