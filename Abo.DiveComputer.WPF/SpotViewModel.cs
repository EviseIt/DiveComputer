using System.Windows.Media;
using RealWorldPlot.Interfaces;

namespace RealWorldPlotter;

public class SpotViewModel<S>
    where S : RealWorldPoint

{
    public Color Color { get; }
    public double Radius { get; }
    public S RealWorldPoint { get; }

    public SpotViewModel(S realWorldPoint,Color color, double radius)
    {
        Color = color;
        Radius = radius;
        RealWorldPoint = realWorldPoint;
    }
}