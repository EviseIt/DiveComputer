namespace RealWorldPlot.Interfaces;

public struct PointD(double x, double y)
{
    public double X { get; } = x;
    public double Y { get; } = y;
    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}