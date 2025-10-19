using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
/// <summary>
/// Points dessinés. Les points n’appartiennent pas à la courbe
/// </summary>
public class SpotCollection : AbstractSpotCollection<RealWorldPoint>
{
}
/// <summary>
/// Points mis en évidence. Les points appartiennent à la courbe
/// </summary>
public class HighlightCollection : AbstractSpotCollection<IndexedRealWorldPoint>
{
}

public partial class RealWorldPlot : Canvas,INotifyPropertyChanged
{
    private readonly SpotCollection _spotCollection = new();
    public void SpotPoint(RealWorldPoint realWorldPoint,Color color,double radius)
    {
        if (_curvesByKey.Count > 0)
        {
            if (_spotCollection != null && !_spotCollection.Contains(realWorldPoint))
            {
                _spotCollection.AddSpot(realWorldPoint, color, radius);
            }
        }
    }
    //public void Highlight(Guid pointIndex)
    //{
    //    if (_curvesByKey.Count > 0)
    //    {
    //        IndexedRealWorldPoint? indexedRealWorldPoint=  _curvesByKey[MainCurve.Main].Points.FindById(pointIndex);
    //        if (indexedRealWorldPoint != null && !_highlightedPoints.Contains(indexedRealWorldPoint))
    //        {
    //            _highlightedPoints.Add(indexedRealWorldPoint)
    //        }
    //    }
    //}

}