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

public partial class RealWorldPlot : Canvas,INotifyPropertyChanged
{
    public class BoundChangedEventArgs
    {
        public double NewBoundValue { get; set; }
        public Curve OutOfBoundCurve { get; set; }
    }

    public delegate void OnBoundChanged(object sender, BoundChangedEventArgs args);
    public event OnBoundChanged OnXMaxChanged;

    private IndexedRealWorldPoint? _selectedPoint = null;

    public double XMin
    {
        get { return _xMin; }
        set
        {
            if (_xMin != value)
            {
                _xMin = value;
                OnPropertyChanged();
                _refreshVisual();
            }
        }
    }

    public double XMax
    {
        get { return _xMax; }
        set
        {
            if (_xMax != value)
            {
                _xMax = value;
                OnPropertyChanged();
                if (BoundTriming)
                {
                    foreach (var curve in _curvesByKey.Values)
                    {
                        curve.TrimXMax(_xMax);
                    }
                }

                _refreshVisual();
            }
        }
    }

    public double YMin
    {
        get { return _yMin; }
        set
        {
            if (_yMin != value)
            {
                _yMin = value;
                OnPropertyChanged();
                if (BoundTriming)
                {
                    foreach (var curve in _curvesByKey.Values)
                    {
                        curve.TrimYMin(_yMin);
                    }
                }
                _refreshVisual();
            }
        }
    }

    public double YMax
    {
        get { return _yMax; }
        set
        {
            if (_yMax != value)
            {
                _yMax = value;
                OnPropertyChanged();
                _refreshVisual();
            }
        }
    }
    /// <summary>
    /// Point in the real world corresponding to the MinX and MinY values of the plot.
    /// </summary>
    public RealWorldPoint BottomLeft => new RealWorldPoint(XMin, YMin);
    /// <summary>
    /// Point in the real world corresponding to the MaxX and MaxY values of the plot.
    /// </summary>
    public RealWorldPoint TopRight => new RealWorldPoint(XMax, YMax);
    /// <summary>
    /// Point in the real world corresponding to the MaxX and MinY values of the plot.
    /// </summary>
    public RealWorldPoint TopLeft => new RealWorldPoint(XMin, YMax);

    /// <summary>
    /// Point in the real world corresponding to the MaxX and MinY values of the plot.
    /// </summary>
    public RealWorldPoint BottomRight => new RealWorldPoint(XMax, YMin);

    public string XAxisLabel { get; set; } = "X";
    public string YAxisLabel { get; set; } = "Y";

    public event Action<RealWorldPoint> RealWorldClick;

    private readonly ContextMenu _contextMenu;
    private double _xMax=15;
    private double _xMin=-15;
    private double _yMin=-15;
    private double _yMax=15;

    private const double MarginLeft = 60;
    private const double MarginBottom = 40;
    private const double MarginTop = 20;
    private const double MarginRight = 20;

    public RealWorldPlot()
    {
        Background = Brushes.Transparent;

        _contextMenu = new ContextMenu();
        var deleteItem = new MenuItem { Header = "Supprimer le point" };
        deleteItem.Click += (s, e) => RemoveSelectedPoint();
        _contextMenu.Items.Add(deleteItem);
      
    }

   

    

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}