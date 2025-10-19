using RealWorldPlot.Interfaces;
using RealWorldPlot.Interfaces.GeometryHelpers;

namespace Abo.DiveComputer.Core;

public class MValues
{
    public MValues(double a,double b)
    {
        this.AffineLine = new AffineLine(a, b);
        this.Points = new RealWorldPoints();
        RealWorldPoint start = AffineLine.GetY(1);
        RealWorldPoint end = AffineLine.GetY(7);
        this.Points.AddNewPoint(start);
        this.Points.AddNewPoint(end);
    }

    public RealWorldPoints Points { get;}


    public AffineLine AffineLine { get; }
}