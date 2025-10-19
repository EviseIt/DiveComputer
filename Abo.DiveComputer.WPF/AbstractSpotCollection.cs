using System.Windows.Media;
using RealWorldPlot.Interfaces;

namespace RealWorldPlotter;

public class AbstractSpotCollection<S>
    where S: RealWorldPoint
{
    private List<SpotViewModel<S>> _spots = new();
    public IEnumerable<SpotViewModel<S>> Spots => _spots;
    public void AddSpot(S realWorldPoint,Color color,double radius)
    {
        if (realWorldPoint == null) throw new ArgumentNullException(nameof(realWorldPoint));
        if (!Contains(realWorldPoint))
        {
            SpotViewModel<S> spotViewModel = new SpotViewModel<S>(realWorldPoint, color, radius);
            _spots.Add(spotViewModel);
        }
    }
    public bool Contains(S realWorldPoint)
    {
        if (realWorldPoint == null) throw new ArgumentNullException(nameof(realWorldPoint));
        return _spots.Any(s => s.RealWorldPoint.Equals(realWorldPoint));
    }
}