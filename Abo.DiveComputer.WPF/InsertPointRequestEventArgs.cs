using RealWorldPlot.Interfaces;

namespace RealWorldPlotter;

public class InsertPointRequestEventArgs : EventArgs
{
    public RealWorldPoint Point { get; }
    public bool Cancel { get; set; } = false;

    public InsertPointRequestEventArgs(RealWorldPoint point,IndexedRealWorldPoint afterPoint)
    {
        Point = point;
        AfterPoint = afterPoint;
    }

    public IndexedRealWorldPoint AfterPoint { get;  }
}