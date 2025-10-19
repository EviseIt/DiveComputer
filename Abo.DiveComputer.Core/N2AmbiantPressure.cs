using RealWorldPlot.Interfaces;
using RealWorldPlot.Interfaces.GeometryHelpers;

namespace Abo.DiveComputer.Core;




/// <summary>
/// Droite des pressions ambiantes en N2
/// </summary>
public class N2AmbiantPressure
{
    private static N2AmbiantPressure _instance;

    private N2AmbiantPressure()
    {
        this.Start = new RealWorldPoint(1, 0.8);
        this.End = new RealWorldPoint(7, 5.6);
        this.AffineLine = AffineLine.FromPoints(this.Start, this.End);
        this.Points = new RealWorldPoints();
        this.Points.AddNewPoint(this.Start);
        this.Points.AddNewPoint(this.End);
    }

    public AffineLine AffineLine { get; }

    public static N2AmbiantPressure GetInstance()
    {
        if (_instance == null)
        {
            _instance= new N2AmbiantPressure();
        }
        return _instance;
    }

    public RealWorldPoint End { get;  }

    public RealWorldPoint Start { get; }

    public RealWorldPoints Points { get; }
}