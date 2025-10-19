using RealWorldPlot.Interfaces.GeometryHelpers;

namespace RealWorldPlot.Interfaces;

public class RealWorldPoints
{
    public delegate void OnPointAddedHandler(IndexedRealWorldPoint point);

    public event OnPointAddedHandler? OnPointAdded;

    public delegate void OnPointRemovedHandler(IndexedRealWorldPoint point);

    public event OnPointRemovedHandler? OnPointRemoved;

    public delegate void OnDataChangedHandler(RealWorldPoints sender);

    public event OnDataChangedHandler? OnDataChanged;

    private readonly Dictionary<Guid, IndexedRealWorldPoint> _points = new();
    private IndexedRealWorldPoint _firstPoint;
    private IndexedRealWorldPoint _lastPoint;

    public int Count => _points.Count;


    public void Clear()
    {
        _points.Clear();
        _firstPoint = null;
        _lastPoint = null;
    }

    /// <summary>
    /// Supprimer un point du monde réel en utilisant son identifiant.
    /// </summary>
    /// <param name="realWorldPoint"></param>
    public void DeletePoint(IndexedRealWorldPoint realWorldPoint)
    {
        DeletePoint(realWorldPoint.Id);
    }

    /// <summary>
    /// Recherche un point du monde réel par son identifiant unique.
    /// Retourne le point correspondant si trouvé, sinon null.
    /// </summary>
    /// <param name="id">Identifiant unique du point à rechercher.</param>
    /// <returns>Le point trouvé ou null si absent.</returns>
    public IndexedRealWorldPoint? FindById(Guid id)
    {
        IndexedRealWorldPoint? indexedRealWorldPoint = null;
        if (_points.ContainsKey(id))
        {
            indexedRealWorldPoint = _points[id];
        }

        return indexedRealWorldPoint;
    }

    /// <summary>
    /// Supprimer un point du monde réel en utilisant son identifiant.
    /// On ne peut supprimer ni le premier ni le dernier
    /// </summary>
    /// <param name="id"></param>
    public void DeletePoint(Guid id)
    {
        if (_points.ContainsKey(id) && _points.Count > 3)
        {
            IndexedRealWorldPoint? pointToRemove = FindById(id);
            if (pointToRemove.PreviousPoint != null && pointToRemove.NextPoint != null)
            {
                IndexedRealWorldPoint previous = pointToRemove.PreviousPoint;
                IndexedRealWorldPoint next = pointToRemove.NextPoint;
                previous.setNext(next);
                _points.Remove(id);
                OnPointRemoved?.Invoke(pointToRemove);
                OnDataChanged?.Invoke(this);
            }
        }
    }


    /// <summary>
    /// Insère un point après le point spécifié par afterPointReference.
    /// </summary>
    /// <param name="point"></param>
    /// <param name="afterPointReference"></param>
    public IndexedRealWorldPoint InsertPoint(RealWorldPoint point, IndexedRealWorldPoint afterPointReference)
    {
        IndexedRealWorldPoint newRealWorldPoint = new IndexedRealWorldPoint(this, Guid.NewGuid(), point.X, point.Y);

        IndexedRealWorldPoint aLimitPoint = afterPointReference;
        IndexedRealWorldPoint bLimitPoint = afterPointReference.NextPoint;
        aLimitPoint.setNext(newRealWorldPoint);
        newRealWorldPoint.setNext(bLimitPoint);
        _points.Add(newRealWorldPoint.Id, newRealWorldPoint);

        OnPointAdded?.Invoke(newRealWorldPoint);
        //OnDataChanged?.Invoke(this);
        return newRealWorldPoint;
    }



    public double MinWorldX
    {
        get
        {
            if (_points.Count == 0)
                return -1;
            return _points.Values.Min(p => p.X);
        }
    }

    public double MaxWorldX
    {
        get
        {
            if (_points.Count == 0)
                return 1;
            return _points.Values.Max(p => p.X);
        }
    }

    public double MinWorldY
    {
        get
        {
            if (_points.Count == 0)
                return -1;
            return _points.Values.Min(p => p.Y);
        }
    }

    public double MaxWorldY
    {
        get
        {
            if (_points.Count == 0)
                return 1;
            return _points.Values.Max(p => p.Y);
        }
    }

    /// <summary>
    /// il y a-t-il au moins un point dans le monde réel ?
    /// </summary>
    public bool HasAtLeastTwoPoint
    {
        get { return _points.Count > 1; }
    }

    public IndexedSegment GetLastTwoPoints()
    {
        IndexedSegment? segment = null;
        if (HasAtLeastTwoPoint)
        {
            segment = new IndexedSegment(_lastPoint.PreviousPoint, _lastPoint);
        }
        return segment;
    }


    /// <summary>
    /// Enumérer les points du monde réel deux par deux en appelant la fonction de rappel pour chaque point.
    /// </summary>
    /// <param name="enumeration"></param>
    public void EnumerateByTwoPoints(Action<IndexedRealWorldPoint, IndexedRealWorldPoint> enumeration)
    {
        List<IndexedRealWorldPoint> pointList = new();
        IndexedRealWorldPoint current = _firstPoint;
        while (current != null && current.NextPoint != null)
        {
            if (pointList.Contains(current))
                throw new Exception("le point a déjà été enuméré");
            enumeration(current, current.NextPoint);
            pointList.Add(current);
            current = current.NextPoint;
        }
    }

    public IndexedRealWorldPoint FirstPoint
    {
        get => _firstPoint;

    }

    public PointD[] Points
    {
        get
        {
            List<PointD> retValue = new List<PointD>();
            EnumeratePoints(p => retValue.Add(new PointD(p.X, p.Y)));
            return retValue.ToArray();
        }
    }

    /// <summary>
    /// Enumérer les points du monde réel en appelant la fonction de rappel pour chaque point.
    /// </summary>
    /// <param name="enumeration"></param>
    public void EnumeratePoints(Action<IndexedRealWorldPoint> enumeration)
    {

        IndexedRealWorldPoint current = _firstPoint;
        while (current != null)
        {
            enumeration(current);
            current = current.NextPoint;
        }
    }

    public void AddNewPoint(double x, double y)
    {
        AddNewPoint(new RealWorldPoint(x, y));
    }

    private void _addNewPoint(IndexedRealWorldPoint point)
    {
        IndexedRealWorldPoint newPoint = new IndexedRealWorldPoint(this, point.Id, point.X, point.Y);

        if (_points.Count == 0)
        {
            _firstPoint = newPoint;
            _lastPoint = newPoint;
        }
        else
        {

            _lastPoint.setNext(newPoint);
            _lastPoint = newPoint;
        }

        _points.Add(newPoint.Id, newPoint);
        OnPointAdded?.Invoke(newPoint);
        OnDataChanged?.Invoke(this);
    }

    /// <summary>
    /// Ajouter un point du monde réel avec un identifiant unique et des coordonnées X et Y.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void AddNewPoint(RealWorldPoint point)
    {
        IndexedRealWorldPoint newPoint = new IndexedRealWorldPoint(this, Guid.NewGuid(), point.X, point.Y);

        _addNewPoint(newPoint);
    }

    public void AddNewPoint(RealWorldPoint point, Guid knownId)
    {
        IndexedRealWorldPoint newPoint = new IndexedRealWorldPoint(this, knownId, point.X, point.Y);

        _addNewPoint(newPoint);
    }

    public bool FindSurroundingPoints(RealWorldPoint realCoord, out IndexedRealWorldPoint p1, out IndexedRealWorldPoint p2)
    {
        p1 = p2 = null;


        bool found = false;
        IndexedRealWorldPoint current = _firstPoint;
        while (current != null && current.NextPoint != null)
        {
            if (realCoord.X > current.X && realCoord.X < current.NextPoint.X)
            {
                p1 = current;
                p2 = current.NextPoint;
                found = true;
            }

            current = current.NextPoint;
        }

        found = p1 != null && p2 != null && DistancePointToVector2D(p1, p2, realCoord) < 1;
        if (!found)
        {
            p1 = p2 = null;
        }

        return found;
    }

    public RealWorldPoints Clone()
    {
        RealWorldPoints clone = new RealWorldPoints();
        EnumeratePoints(p => clone._addNewPoint(p));
        return clone;
    }

    public static double DistancePointToVector2D(IndexedRealWorldPoint vectorEndA, IndexedRealWorldPoint vectorEndB, RealWorldPoint point)
    {
        double abX = vectorEndB.X - vectorEndA.X;
        double abY = vectorEndB.Y - vectorEndA.Y;
        double apX = point.X - vectorEndA.X;
        double apY = point.Y - vectorEndA.Y;

        // Produit vectoriel 2D (valeur absolue)
        double cross = Math.Abs(abX * apY - abY * apX);

        // Norme du vecteur AB
        double lengthAB = Math.Sqrt(abX * abX + abY * abY);
        if (lengthAB == 0)
            throw new Exception("A et B sont confondus");

        return cross / lengthAB;
    }

    public void dataChanged()
    {
        OnDataChanged?.Invoke(this);
    }

    /// <summary>
    /// Echantillonne les points du monde réel à intervalles réguliers.
    /// Par exemple on Au point (0,0) et au point (10,10) on peut échantillonner tous les 1m.
    /// </summary>
    /// <param name="step"></param>
    /// <returns></returns>
    public RealWorldPoints Sample(double stepX)
    {
        RealWorldPoints sampledPoints = new RealWorldPoints();
        bool firstSegment = true;
        this.EnumerateByTwoPoints((a, b) =>
        {

            AffineLine line = AffineLine.FromPoints(a, b);
            if (firstSegment)
            {
                sampledPoints.AddNewPoint(a, a.Id);
            }

            var intermediatePoints = line.GetIntermediatePoints(a.X, b.X, stepX);
            foreach (var point in intermediatePoints)
            {
                sampledPoints.AddNewPoint(point);
            }

            sampledPoints.AddNewPoint(b, b.Id);
            firstSegment = false;

        });
        return sampledPoints;
    }

    public void RoundXMin(double xMin)
    {
        foreach (var point in _points.Where(x => x.Value.X < xMin))
        {
            point.Value.X = xMin;
        }
    }

    public void RoundXMax(double xMax)
    {
        foreach (var point in _points.Where(x => x.Value.X > xMax))
        {
            point.Value.X = xMax;
        }
    }

    public void RoundYMin(double yMin)
    {
        foreach (var point in _points.Where(x => x.Value.Y < yMin))
        {
            point.Value.Y = yMin;
        }
    }

    public void RoundYMax(double yMax)
    {
        foreach (var point in _points.Where(x => x.Value.Y > yMax))
        {
            point.Value.Y = yMax;
        }
    }
    public void TrimYMin(double yMin)
    {
        if (yMin>MinWorldY)
        {
            foreach (var point in _points.Where(p=>p.Value.Y<yMin).ToArray())
            {
                point.Value.Y=yMin;
            }
        }
    }
    public void TrimXMax(double xMax)
    {
        //Si on réduit la valeur max, on supprime les points au delà de cette valeur
        if (MaxWorldX > xMax)
        {
            var allPointsToDelete = _points.OrderBy(p => p.Value.X).Where(p => p.Value.X > xMax).Select(z=>z.Value).ToArray();
            foreach (var point in allPointsToDelete)
            {
                DeletePoint(point);
            }

            var lastPoint= _points.Values.OrderBy(p => p.X).LastOrDefault();
            if (lastPoint != null)
            {
                lastPoint.X = xMax;
            }
        }
        else if (MaxWorldX < xMax)
        {
            //le dernier point vaut max X
            var lastPoint = _points.Values.OrderBy(p => p.X).LastOrDefault();
            if (lastPoint != null)
            {
                lastPoint.X = xMax;
            }
        }
    }
}