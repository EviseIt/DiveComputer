using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using RealWorldPlot.Interfaces;

namespace RealWorldPlotter;

public partial class RealWorldPlot : Canvas,INotifyPropertyChanged
{
   
   

    private RealWorldPoints _mainCurvePoints;
    /// <summary>
    /// Données statiques, non modifiables
    /// </summary>
    private Dictionary<RealWorldPoints,PenInfo> _staticData=new();
    /// <summary>
    /// Données statiques, non modifiables de polygones
    /// </summary>
    private Dictionary<RealWorldPoints, BrushInfo> _staticPolys = new();

    private readonly Dictionary<object,Curve> _curvesByKey=new();
    private bool _beginAppendPoints;

    private void _onPointRemoved(IndexedRealWorldPoint point)
    {
        _refreshVisual();
    }

    private void _onPointAdded(IndexedRealWorldPoint point)
    {
        _refreshVisual();
    }
    /// <summary>
    /// Ajoute des courbes statiques à la visualisation.On ne peut pas ajouter de points à ces courbes, elles sont considérées comme des données de référence.
    /// </summary>
    /// <param name="realWorldPoints"></param>
    /// <param name="penInfo"></param>
    public void AppendStaticData(RealWorldPoints realWorldPoints, PenInfo penInfo)
    {
        _staticData.Add(realWorldPoints, penInfo);
    }
    /// <summary>
    /// Ajoute des figures statiques à la visualisation.On ne peut pas ajouter de points à ces courbes, elles sont considérées comme des données de référence.
    /// </summary>
    /// <param name="realWorldPoints"></param>
    /// <param name="penInfo"></param>
    public void AppendStaticPoly(RealWorldPoints realWorldPoints, BrushInfo brushInfo)
    {
        _staticPolys.Add(realWorldPoints, brushInfo);
    }
    /// <summary>
    /// Définit les données modulables
    /// </summary>
    /// <param name="realWorldPoints"></param>
    /// <param name="penInfo"></param>
    public void SetPoints(RealWorldPoints realWorldPoints,PenInfo penInfo,bool autoMinMax=true)
    {


        _curvesByKey.Clear();
        _curvesByKey.Add(MainCurve.Main, new Curve(realWorldPoints,penInfo));
        _mainCurvePoints=realWorldPoints;
        //_mainPen = penInfo.ToPen();

        //_points=realWorldPoints;
        //_points.OnPointAdded += _onPointAdded;
        //_points.OnPointRemoved += _onPointRemoved;

        realWorldPoints.OnPointAdded += _onPointAdded;
        realWorldPoints.OnPointRemoved += _onPointRemoved;

        _doLockInvalidate();


        if (autoMinMax)
        {
            XMax = realWorldPoints.MaxWorldX;
            XMin = realWorldPoints.MinWorldX;
            YMax = realWorldPoints.MaxWorldY;
            YMin = realWorldPoints.MinWorldY;
        }

        _doUnlockInvalidate();
        _refreshVisual();
    }
    /// <summary>
    /// Objet articifiel pour servir de clé à la courbe principale
    /// </summary>
    public enum MainCurve
    {
        Main
    }
    /// <summary>
    /// Commence l'ajout de courbes multiples. La courbe passée en paramètre est la courbe principale, celle dont on peut selectionner les points.
    /// </summary>
    /// <param name="realWorldPoints"></param>
    /// <param name="penInfo"></param>
    /// <param name="autoMinMax"></param>
    public void BeginAppendPoints()
    {
        if (_beginAppendPoints)
        {
            throw new Exception($"{nameof(BeginAppendPoints)} has already been called but not {nameof(EndBeginAppendPoints)}");
        }
        _doLockInvalidate();
        _curvesByKey.Clear();
        _beginAppendPoints = true;
    }
    public void AppendPoints(object key,RealWorldPoints realWorldPoints, PenInfo penInfo, bool autoMinMax = true)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));
        if (realWorldPoints == null) throw new ArgumentNullException(nameof(realWorldPoints));
        if (penInfo == null) throw new ArgumentNullException(nameof(penInfo));
        if (!_beginAppendPoints)
        {
            throw new Exception($"{nameof(BeginAppendPoints)} has not been called");
        }
        _doLockInvalidate();
        if (_curvesByKey.Count == 0)
        {
            _mainCurvePoints=realWorldPoints;
        }
        _curvesByKey.Add(key, new Curve(realWorldPoints, penInfo));
        if (autoMinMax)
        {
            if (XMax < realWorldPoints.MaxWorldX)
                XMax = realWorldPoints.MaxWorldX;
            if (XMin > realWorldPoints.MinWorldX)
                XMin = realWorldPoints.MinWorldX;
            if (YMax < realWorldPoints.MaxWorldY)
                YMax = realWorldPoints.MaxWorldY;
            if (YMin > realWorldPoints.MinWorldY)
                YMin = realWorldPoints.MinWorldY;
        }
    }
    public void EndBeginAppendPoints()
    {
        if (!_beginAppendPoints)
        {
            throw new Exception($"{nameof(BeginAppendPoints)} has not been called");
        }
        _doUnlockInvalidate();
        _refreshVisual();
        _beginAppendPoints = false;
    }

    public void RemoveSelectedPoint()
    {
        if (_selectedPoint!=null)
        {
            _mainCurvePoints.DeletePoint(_selectedPoint);
            _selectedPoint = null;
            _refreshVisual();
        }
    }

    public delegate void InsertPointRequestHandler(object sender,InsertPointRequestEventArgs point);
    public event InsertPointRequestHandler OnInsertPointRequest;

    public delegate void PointSelectedHandler(object sender, PointSelectedEventArgs point);
    public event PointSelectedHandler OnPointSelectedHandler;

    private Point RealToPixel(RealWorldPoint p, double plotWidth, double plotHeight)
    {
        double x = MarginLeft + (p.X - XMin) / (XMax - XMin) * plotWidth;
        double y = MarginTop + (1 - (p.Y - YMin) / (YMax - YMin)) * plotHeight;
        return new Point(x, y);
    }

    private RealWorldPoint PixelToRealUnclamped(Point pixel)
    {
        double plotWidth = ActualWidth - MarginLeft - MarginRight;
        double plotHeight = ActualHeight - MarginTop - MarginBottom;
        double x = XMin + (pixel.X - MarginLeft) / plotWidth * (XMax - XMin);
        double y = YMin + (1 - (pixel.Y - MarginTop) / plotHeight) * (YMax - YMin);
        return new RealWorldPoint(x, y);
    }

    public class Curve
    {
        public RealWorldPoints Points { get;  }
        public PenInfo PenInfo { get;  }

        public Curve(RealWorldPoints points, PenInfo penInfo)
        {
            Points = points;
            PenInfo = penInfo;
        }
        /// <summary>
        ///Remettre tous les points au maximum X
        /// </summary>
        public void TrimXMax(double xMax)
        {
            Points.TrimXMax(xMax);
        }
        public void TrimYMin(double yMin)
        {
            Points.TrimYMin(yMin);
        }
    }
   

   
}