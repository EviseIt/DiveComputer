using RealWorldPlot.Interfaces;

namespace RealWorldPlotter;

public class PointSelectedEventArgs:EventArgs
{
    public PointSelectedEventArgs(IndexedRealWorldPoint selectedPoint)
    {
        SelectedPoint = selectedPoint;
    }

    public IndexedRealWorldPoint SelectedPoint { get; }
}