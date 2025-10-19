//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Input;
//using System.Windows.Media;
//using RealWorldPlot.Interfaces;

//namespace RealWorldPlotter;

//public class RealWorldPlot0 : Canvas
//{
//    private bool _lockInvalidate = false; // Flag to control invalidation
//    private bool _isDragging = false;
//    private Point _dragStartPixel;
//    public double SelectionTolerance { get; set; } = 20;
//    private IndexedRealWorldPoint? _selectedPoint = null;
//    public double XMin { get; set; } = 15;
//    public double XMax
//    {
//        get { return _xMax; }
//        set
//        {
//            if (_xMax != value)
//            {
//                _xMax = value;
//                _refreshVisual();
//            }
//        }
//    }
//    public double YMin { get; set; } = 0;
//    public double YMax { get; set; } = 10;

//    public string XAxisLabel { get; set; } = "X";
//    public string YAxisLabel { get; set; } = "Y";

//    public event Action<IndexedRealWorldPoint> RealWorldClick;

//    private List<IndexedRealWorldPoint> _points = new();
//    private ContextMenu _contextMenu;
//    private double _xMax;

//    private const double MarginLeft = 60;
//    private const double MarginBottom = 40;
//    private const double MarginTop = 20;
//    private const double MarginRight = 20;

//    public RealWorldPlot0()
//    {
//        Background = Brushes.Transparent;

//        _contextMenu = new ContextMenu();
//        var deleteItem = new MenuItem { Header = "Supprimer le point" };
//        deleteItem.Click += (s, e) => RemoveSelectedPoint();
//        _contextMenu.Items.Add(deleteItem);
//    }

//    public void SetPoints(List<IndexedRealWorldPoint> realWorldPoints)
//    {
//        _points = realWorldPoints;
//        _doLockInvalidate();
//        XMax = realWorldPoints.Select(p => p.X).Max();
//        XMin = realWorldPoints.Select(p => p.X).Min();
//        YMax = realWorldPoints.Select(p => p.Y).Max();
//        YMin = realWorldPoints.Select(p => p.Y).Min();
//        _doUnlockInvalidate();
//        _refreshVisual();
//    }

//    private void _refreshVisual()
//    {
//        if (!_lockInvalidate)
//        {
//            InvalidateVisual();
//        }
//    }
//    private void _doUnlockInvalidate()
//    {
//        _lockInvalidate = false;
//    }

//    private void _doLockInvalidate()
//    {
//        _lockInvalidate = true;
//    }

//    public void RemoveSelectedPoint()
//    {
//        if (_selectedPoint!=null)
//        {
//            _points.RemoveAll(p => Math.Abs(p.X - _selectedPoint.X) < 1e-6 &&
//                                   Math.Abs(p.Y - _selectedPoint.Y) < 1e-6);
//            _selectedPoint = null;
//            _refreshVisual();
//        }
//    }

//    protected override void OnRender(DrawingContext dc)
//    {
//        base.OnRender(dc);

//        double width = ActualWidth;
//        double height = ActualHeight;
//        double plotWidth = width - MarginLeft - MarginRight;
//        double plotHeight = height - MarginTop - MarginBottom;

//        var penAxes = new Pen(Brushes.Black, 1);
//        var penGrid = new Pen(Brushes.LightGray, 0.5);
//        var penData = new Pen(Brushes.Blue, 1.5);
//        var ftStyle = new Typeface("Segoe UI");

//        // Grille + graduations
//        for (int i = 0; i <= 10; i++)
//        {
//            double x = XMin + i * (XMax - XMin) / 10;
//            double xPixel = MarginLeft + (x - XMin) / (XMax - XMin) * plotWidth;
//            dc.DrawLine(penGrid, new Point(xPixel, MarginTop), new Point(xPixel, height - MarginBottom));
//            var ft = new FormattedText($"{x:0.##}", System.Globalization.CultureInfo.InvariantCulture,
//                FlowDirection.LeftToRight, ftStyle, 10, Brushes.Black, VisualTreeHelper.GetDpi(this).PixelsPerDip);
//            dc.DrawText(ft, new Point(xPixel - ft.Width / 2, height - MarginBottom + 2));
//        }

//        for (int i = 0; i <= 10; i++)
//        {
//            double y = YMin + i * (YMax - YMin) / 10;
//            double yPixel = MarginTop + (1 - (y - YMin) / (YMax - YMin)) * plotHeight;
//            dc.DrawLine(penGrid, new Point(MarginLeft, yPixel), new Point(width - MarginRight, yPixel));
//            var ft = new FormattedText($"{y:0.##}", System.Globalization.CultureInfo.InvariantCulture,
//                FlowDirection.LeftToRight, ftStyle, 10, Brushes.Black, VisualTreeHelper.GetDpi(this).PixelsPerDip);
//            dc.DrawText(ft, new Point(MarginLeft - ft.Width - 5, yPixel - ft.Height / 2));
//        }

//        // Axes
//        dc.DrawLine(penAxes, new Point(MarginLeft, height - MarginBottom), new Point(width - MarginRight, height - MarginBottom));
//        dc.DrawLine(penAxes, new Point(MarginLeft, MarginTop), new Point(MarginLeft, height - MarginBottom));

//        // Légendes axes
//        var xLabel = new FormattedText(XAxisLabel, System.Globalization.CultureInfo.InvariantCulture,
//            FlowDirection.LeftToRight, ftStyle, 12, Brushes.Black, VisualTreeHelper.GetDpi(this).PixelsPerDip);
//        dc.DrawText(xLabel, new Point(MarginLeft + (plotWidth - xLabel.Width) / 2, height - xLabel.Height));

//        var yLabel = new FormattedText(YAxisLabel, System.Globalization.CultureInfo.InvariantCulture,
//            FlowDirection.LeftToRight, ftStyle, 12, Brushes.Black, VisualTreeHelper.GetDpi(this).PixelsPerDip);
//        dc.PushTransform(new TranslateTransform(5, MarginTop + (plotHeight + yLabel.Width) / 2));
//        dc.PushTransform(new RotateTransform(-90));
//        dc.DrawText(yLabel, new Point(0, 0));
//        dc.Pop();
//        dc.Pop();

//        // Courbe
//        if (_points.Count > 1)
//        {
//            for (int i = 0; i < _points.Count - 1; i++)
//            {
//                dc.DrawLine(penData,
//                    RealToPixel(_points[i], plotWidth, plotHeight),
//                    RealToPixel(_points[i + 1], plotWidth, plotHeight));
//            }
//        }

//        // Point sélectionné
//        if (_selectedPoint!=null)
//        {
//            var sel = RealToPixel(_selectedPoint, plotWidth, plotHeight);
//            dc.DrawEllipse(Brushes.Red, new Pen(Brushes.Red, 1), sel, 5, 5);
//        }
//    }

//    protected override void OnMouseDown(MouseButtonEventArgs e)
//    {
//        base.OnMouseDown(e);
//        Point clicked = e.GetPosition(this);

//        double plotWidth = ActualWidth - MarginLeft - MarginRight;
//        double plotHeight = ActualHeight - MarginTop - MarginBottom;
//        RealWorldPoint realCoord = PixelToRealUnclamped(clicked);

//        // Insertion Ctrl + clic gauche
//        if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control &&
//            e.ChangedButton == MouseButton.Left)
//        {
//            for (int i = 0; i < _points.Count - 1; i++)
//            {
//                var p1 = _points[i];
//                var p2 = _points[i + 1];

//                if (realCoord.X > p1.X && realCoord.X < p2.X)
//                {
//                    _points.Insert(i + 1, realCoord);
//                    _selectedPoint = realCoord;
//                    _refreshVisual();
//                    return;
//                }
//            }
//            return; // insertion ignorée si abscisse non valide
//        }

//        // Sélection d’un point proche
//        IndexedRealWorldPoint? best = null;
//        double minDist = double.MaxValue;

//        foreach (var p in _points)
//        {
//            double dist = (RealToPixel(p, plotWidth, plotHeight) - clicked).LengthSquared;
//            if (dist < minDist)
//            {
//                minDist = dist;
//                best = p;
//            }
//        }

//        if (best!=null && minDist <= SelectionTolerance * SelectionTolerance)
//        {
//            _selectedPoint = best;

//            if (e.ChangedButton == MouseButton.Right)
//            {
//                _contextMenu.PlacementTarget = this;
//                _contextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.MousePoint;
//                _contextMenu.IsOpen = true;
//            }
//            else if (e.ChangedButton == MouseButton.Left)
//            {
//                _isDragging = true;
//                _dragStartPixel = clicked;
//                CaptureMouse();
//            }
//        }
//        else
//        {
//            _selectedPoint = null;
//            _isDragging = false;
//            _contextMenu.IsOpen = false;
//        }

//        if (clicked.X >= MarginLeft && clicked.Y >= MarginTop &&
//            clicked.X <= ActualWidth - MarginRight && clicked.Y <= ActualHeight - MarginBottom)
//        {
//            RealWorldClick?.Invoke(realCoord);
//        }

//        _refreshVisual();
//    }

//    protected override void OnMouseMove(MouseEventArgs e)
//    {
//        base.OnMouseMove(e);

//        if (_isDragging && _selectedPoint!=null)
//        {
//            Point pos = e.GetPosition(this);
//            IndexedRealWorldPoint newReal = PixelToRealUnclamped(pos);

//            int index = _points.FindIndex(p => Math.Abs(p.X - _selectedPoint.X) < 1e-6 &&
//                                               Math.Abs(p.Y - _selectedPoint.Y) < 1e-6);
//            if (index >= 0)
//            {
//                // Vérifie contrainte d’abscisse entre voisins
//                bool valid =
//                    (index == 0 || newReal.X > _points[index - 1].X) &&
//                    (index == _points.Count - 1 || newReal.X < _points[index + 1].X);

//                if (valid)
//                {
//                    _points[index] = newReal;
//                    _selectedPoint = newReal;
//                    _refreshVisual();
//                }
//            }
//        }
//    }

//    protected override void OnMouseUp(MouseButtonEventArgs e)
//    {
//        base.OnMouseUp(e);
//        if (_isDragging)
//        {
//            _isDragging = false;
//            ReleaseMouseCapture();
//        }
//    }

//    private Point RealToPixel(IndexedRealWorldPoint p, double plotWidth, double plotHeight)
//    {
//        double x = MarginLeft + (p.X - XMin) / (XMax - XMin) * plotWidth;
//        double y = MarginTop + (1 - (p.Y - YMin) / (YMax - YMin)) * plotHeight;
//        return new Point(x, y);
//    }

//    private RealWorldPoint PixelToRealUnclamped(Point pixel)
//    {
//        double plotWidth = ActualWidth - MarginLeft - MarginRight;
//        double plotHeight = ActualHeight - MarginTop - MarginBottom;
//        double x = XMin + (pixel.X - MarginLeft) / plotWidth * (XMax - XMin);
//        double y = YMin + (1 - (pixel.Y - MarginTop) / plotHeight) * (YMax - YMin);
//        return new RealWorldPoint(x, y);
//    }
//}