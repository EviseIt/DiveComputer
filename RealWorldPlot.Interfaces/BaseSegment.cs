namespace RealWorldPlot.Interfaces;

/// <summary>
/// Two points
/// </summary>
public abstract class BaseSegment<P>
    where P:RealWorldPoint
{
    public BaseSegment(P pointA,P pointB)
    {

        this.PointA = pointA;
        this.PointB = pointB;
    }

    public P PointB { get; }

    public P PointA { get; }
    protected bool contains(double xa,double xb,double ya,double yb,double x,double y)
    {
        double xMin = Math.Min(xa, xb);
        double xMax = Math.Max(xa, xb);
        double yMin = Math.Min(ya, yb);
        double yMax = Math.Max(ya, yb);
        return x >= xMin && x <= xMax && y >= yMin && y <= yMax;
    }
    public bool Contains(P inter)
    {
        return contains(PointA.X, PointB.X, PointA.Y, PointB.Y, inter.X, inter.Y);
    }
}