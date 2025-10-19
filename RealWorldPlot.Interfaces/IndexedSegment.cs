namespace RealWorldPlot.Interfaces;

public class IndexedSegment:BaseSegment<IndexedRealWorldPoint>
{
    public IndexedSegment(IndexedRealWorldPoint pointA, IndexedRealWorldPoint pointB) : base(pointA, pointB)
    {
    }

    public Segment ToSegment()
    {
        return new Segment(new RealWorldPoint(PointA.X, PointA.Y), new RealWorldPoint(PointB.X, PointB.Y));
    }
}