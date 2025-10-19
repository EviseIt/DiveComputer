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

public partial class RealWorldPlot : Canvas, INotifyPropertyChanged
{
    private bool _lockInvalidate = false; // Flag to control invalidation
    private Pen _mainPen;

    public SelectionVisualInfo SelectionVisualInfo { get; set; } = new SelectionVisualInfo();

    private void _refreshVisual()
    {
        if (!_lockInvalidate)
        {
            InvalidateVisual();
        }
    }
    private void _doUnlockInvalidate()
    {
        _lockInvalidate = false;
    }

    private void _doLockInvalidate()
    {
        _lockInvalidate = true;
    }

    public Graduation XGraduation { get; set; } = new Graduation();
    public Graduation YGraduation { get; set; } = new Graduation();

    protected override void OnRender(DrawingContext dc)
    {
        base.OnRender(dc);

        double width = ActualWidth;
        double height = ActualHeight;
        double plotWidth = width - MarginLeft - MarginRight;
        double plotHeight = height - MarginTop - MarginBottom;

        var penAxes = new Pen(Brushes.Black, 1);
        var penGrid = new Pen(Brushes.LightGray, 0.5);
        var ftStyle = new Typeface("Segoe UI");


        var xSteps = XGraduation.GetGraduation(XMin, XMax);
        // Grille + graduations
        //for (int i = 0; i <= 10; i++)
        //{
        //    double x = XMin + i * (XMax - XMin) / 10;
        for (int i = 0; i < xSteps.Length; i++)
        {
            double x = xSteps[i];

            double xPixel = MarginLeft + (x - XMin) / (XMax - XMin) * plotWidth;
            dc.DrawLine(penGrid, new Point(xPixel, MarginTop), new Point(xPixel, height - MarginBottom));
            var ft = new FormattedText($"{x:0.##}", System.Globalization.CultureInfo.InvariantCulture,
                FlowDirection.LeftToRight, ftStyle, 10, Brushes.Black, VisualTreeHelper.GetDpi(this).PixelsPerDip);
            dc.DrawText(ft, new Point(xPixel - ft.Width / 2, height - MarginBottom + 2));
        }
        var ySteps = YGraduation.GetGraduation(YMin, YMax);

        for (int i = 0; i < ySteps.Length; i++)
        {
            double y = ySteps[i];
            double yPixel = MarginTop + (1 - (y - YMin) / (YMax - YMin)) * plotHeight;
            dc.DrawLine(penGrid, new Point(MarginLeft, yPixel), new Point(width - MarginRight, yPixel));
            var ft = new FormattedText($"{y:0.##}", System.Globalization.CultureInfo.InvariantCulture,
                FlowDirection.LeftToRight, ftStyle, 10, Brushes.Black, VisualTreeHelper.GetDpi(this).PixelsPerDip);
            dc.DrawText(ft, new Point(MarginLeft - ft.Width - 5, yPixel - ft.Height / 2));
        }

        // Axes
        dc.DrawLine(penAxes, new Point(MarginLeft, height - MarginBottom), new Point(width - MarginRight, height - MarginBottom));
        dc.DrawLine(penAxes, new Point(MarginLeft, MarginTop), new Point(MarginLeft, height - MarginBottom));

        // Légendes axes
        var xLabel = new FormattedText(XAxisLabel, System.Globalization.CultureInfo.InvariantCulture,
            FlowDirection.LeftToRight, ftStyle, 12, Brushes.Black, VisualTreeHelper.GetDpi(this).PixelsPerDip);
        dc.DrawText(xLabel, new Point(MarginLeft + (plotWidth - xLabel.Width) / 2, height - xLabel.Height));

        var yLabel = new FormattedText(YAxisLabel, System.Globalization.CultureInfo.InvariantCulture,
            FlowDirection.LeftToRight, ftStyle, 12, Brushes.Black, VisualTreeHelper.GetDpi(this).PixelsPerDip);
        dc.PushTransform(new TranslateTransform(5, MarginTop + (plotHeight + yLabel.Width) / 2));
        dc.PushTransform(new RotateTransform(-90));
        dc.DrawText(yLabel, new Point(0, 0));
        dc.Pop();
        dc.Pop();

        foreach (var staticPoly in _staticPolys)
        {
            if (staticPoly.Key.HasAtLeastTwoPoint)
            {
                Brush staticBrush = staticPoly.Value.ToBrush();
                // Définir les points du polygone
                StreamGeometry geometry = new StreamGeometry();
                using (StreamGeometryContext ctx = geometry.Open())
                {
                    var current = staticPoly.Key.FirstPoint;
                    ctx.BeginFigure(RealToPixel(current, plotWidth, plotHeight), true, true); // isFilled=true, isClosed=true
                    while (current.NextPoint != null)
                    {
                        current = current.NextPoint;
                        ctx.LineTo(RealToPixel(current, plotWidth, plotHeight), true, false);
                    }
                }
                geometry.Freeze();
                // Dessiner le polygone
                dc.DrawGeometry(staticBrush, null, geometry);
            }
        }


        foreach (var staticData in _staticData)
        {
            if (staticData.Key.HasAtLeastTwoPoint)
            {
                Pen staticPen = staticData.Value.ToPen();
                staticData.Key.EnumerateByTwoPoints((pPrevious, pNext) =>
                {
                    dc.DrawLine(staticPen, RealToPixel(pPrevious, plotWidth, plotHeight), RealToPixel(pNext, plotWidth, plotHeight));
                });
            }
        }

        foreach (var curve in _curvesByKey.Values)
        {
            if (curve.Points != null && curve.Points.HasAtLeastTwoPoint)
            {
                curve.Points.EnumerateByTwoPoints((pPrevious, pNext) =>
                {

                    var z=pPrevious;
                    var x = pNext;

                    dc.DrawLine(curve.PenInfo.ToPen(), RealToPixel(pPrevious, plotWidth, plotHeight), RealToPixel(pNext, plotWidth, plotHeight));
                });
            }
        }
        // Point sélectionné
        if (_selectedPoint != null)
        {
            var sel = RealToPixel(_selectedPoint, plotWidth, plotHeight);
            SolidColorBrush selectionBrush = new SolidColorBrush(SelectionVisualInfo.Color);
            dc.DrawEllipse(selectionBrush, new Pen(selectionBrush, 1), sel, SelectionVisualInfo.Radius, SelectionVisualInfo.Radius);
        }
        //Points dessinés
        foreach (var spot in _spotCollection.Spots)
        {
            var sel = RealToPixel(spot.RealWorldPoint, plotWidth, plotHeight);
            SolidColorBrush spotBrush = new SolidColorBrush(spot.Color);
            dc.DrawEllipse(spotBrush, new Pen(spotBrush, 1), sel, spot.Radius, spot.Radius);
        }
    }

}
/// <summary>
/// Indique les informations de graduation
/// </summary>
public class Graduation
{
    private double _from = -10;
    private double _step = -10;

    /// <summary>
    /// L'échelle est divisée en 10. Ex: de -20 à 20, l'échelle est de 4, donc les graduations sont -20, -16, -12, -8, -4, 0, 4, 8, 12, 16, 20
    /// </summary>
    public Graduation()
    {

    }
    /// <summary>
    /// Initialise une graduation à partir d'une valeur de départ et d'un pas.
    /// Ex: Graduation(0, 5) donnera les graduations -20 ,-15, 0, 5, 10, 15, 20, etc.
    /// </summary>
    /// <param name="from"></param>
    /// <param name="step"></param>
    public Graduation(double from, double step)
    {
        _from = from;
        _step = Math.Abs(step);
    }

    public double[] GetGraduation(double min, double max)
    {
        List<double> graduations = new List<double>();
        if (_step < 0)
        {
            //Graduation auto
            _step = (max - min) / 10;
            _from = min;
        }

        if (_step == 0)
        {
            _step = Math.Abs(min) / 10;
            min = min - _step;
            max = max + _step;
        }


        graduations.Add(_from);
        double current = _from;
        while (current >= min)
        {
            graduations.Insert(0, current);
            current -= _step;

        }

        current = _from + _step;
        while (current <= max)
        {
            graduations.Add(current);
            current += _step;
        }

        return graduations.ToArray();
    }
}

public class SelectionVisualInfo
{
    public double Radius { get; set; } = 5;
    public Color Color { get; set; } = Colors.Red;

}