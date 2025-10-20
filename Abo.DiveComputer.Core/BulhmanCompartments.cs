using System.Collections;
using RealWorldPlot.Interfaces;
using RealWorldPlot.Interfaces.GeometryHelpers;

namespace Abo.DiveComputer.Core;

public class BulhmanCompartments : IEnumerable<BulhmanCompartment>
{

    internal const double ln2 = 0.6931471805599453;
    // Paramètres physiologiques
    internal static double FN2 { get; } = 0.79;     // fraction N2 inspirée (air=0.79)
    internal static double PH2O { get; } = 0.0627;  // bar (vapeur d'eau alvéolaire)

    internal static double PSurfaceBar
    {
        get
        {
            double retValue;
            if (BulhmanCompartments.DEBUG)
            {
                retValue = 1.0;
            }
            else
            {
                retValue = 1.013;
            }

            return retValue;
        }
    }

    public GradientFactorsSettings GradientFactorsSettings
    {
        get;
        set;
    } = GradientFactorsSettings.Default;


    private readonly BulhmanCompartment[] _compartments;
    public BulhmanCompartments(Diver diver)
    {
        this.Diver = diver;
        _compartments = new BulhmanCompartment[]
        {

            new BulhmanCompartment(4,1.9082,1.2599),
            new BulhmanCompartment(5,1.7928,1.1696),
            new BulhmanCompartment(8,1.5352,1),
            new BulhmanCompartment(12.5,1.3847,0.8618),
            new BulhmanCompartment(18.5,1.278,0.7562),
            new BulhmanCompartment(27,1.2306,0.62),
            new BulhmanCompartment(38.3,1.1857,0.5043),
            new BulhmanCompartment(54.3,1.1504,0.441),
            new BulhmanCompartment(77,1.1223,0.375),
            new BulhmanCompartment(109,1.0999,0.35),
            new BulhmanCompartment(146,1.0844,0.3295),
            new BulhmanCompartment(187,1.0731,0.3065),
            new BulhmanCompartment(239,1.0635,0.2835),
            new BulhmanCompartment(305,1.0552,0.285),
            new BulhmanCompartment(390,1.0478,0.261),
            new BulhmanCompartment(498,1.0414,0.248),
            new BulhmanCompartment(635,1.0359,0.2327),


        };



    }

    public Diver Diver { get; }

    /// <summary>
    /// Tensions par compartiment
    /// </summary>
    private readonly Dictionary<BulhmanCompartment, RealWorldPoints> _tensionByCompartment = new();
    /// <summary>
    /// Donnnées de la courbe de plongée sur le graph des M-values pour un compartiment donné.
    /// </summary>
    private readonly Dictionary<BulhmanCompartment, RealWorldPoints> _tensionByAmbiantPressureByCompartment = new();
    public RealWorldPoints Ndl = new();
    public static bool DEBUG = false;
    public BulhmanCompartment this[int index] => _compartments[index];

    public RealWorldPoints GetTensions(BulhmanCompartment compartment)
    {
        return _tensionByCompartment[compartment];
    }
    /// <summary>
    /// Obtient les données de la courbe à de plongée sur le graph des M-values pour un compartiment donné.
    /// </summary>
    /// <param name="compartment"></param>
    /// <returns></returns>
    public RealWorldPoints GetMValuesData(BulhmanCompartment compartment)
    {
        return _tensionByAmbiantPressureByCompartment[compartment];
    }
    private DirectorCompartment _moveTo(Guid diveProfilePointId, double timeEllapsedInMinutes, double depth)
    {
        DirectorCompartment director = DirectorCompartment.None;
        var start = DateTime.Now;

        var compartments = _compartments;
        //if (DEBUG)
        //{
        //    compartments = new Compartment[] { _compartments[0] };
        //}

        foreach (var compartment in compartments)
        {
            BulhmanCompartmentValue compartmentValue = compartment.MoveTo(diveProfilePointId, 60 * timeEllapsedInMinutes, depth);
            double n2Tension = compartmentValue.TensionN2;
            double ambPressure = (Math.Abs(depth) / 10.0) + BulhmanCompartments.PSurfaceBar;

            DateTime start0 = DateTime.Now;
            //Ajouter les points de tension dans le graphe par compartiment
            if (!_tensionByCompartment.TryGetValue(compartment, out RealWorldPoints? compartmentData))
            {
                compartmentData = new RealWorldPoints();
                _tensionByCompartment.Add(compartment, compartmentData);

            }
            compartmentData.AddNewPoint(timeEllapsedInMinutes, n2Tension);

            director.ComputeDirector(compartment, n2Tension, compartmentValue.Ndl);

            //Ajouter les points de la courbe de plongée sur le graph des M-values pour un compartiment donné.
            if (!_tensionByAmbiantPressureByCompartment.TryGetValue(compartment, out RealWorldPoints? tensionByAmbiantPressureByCompartmentData))
            {
                tensionByAmbiantPressureByCompartmentData = new RealWorldPoints();
                _tensionByAmbiantPressureByCompartment.Add(compartment, tensionByAmbiantPressureByCompartmentData);
            }
            tensionByAmbiantPressureByCompartmentData.AddNewPoint(ambPressure, n2Tension);


            IndexedSegment? segment = tensionByAmbiantPressureByCompartmentData.GetLastTwoPoints();
            //si intersection avec Gradientfactpr=>plafond
            //ToDo
            //if (segment != null)
            //{
            //    //remplacer le temps par la pression ambiante
            //    if (compartment._halfLife == 12.5)
            //    {

            //    }
            //    IntersectionResult intersectionResult = EuclidianComputer.FindIntersection(compartment.GradientFactorLine.Low, compartment.GradientFactorLine.High, segment.PointA, segment.PointB);
            //    if (intersectionResult.Intersects && intersectionResult.Point.X > 1)
            //    {
            //        var inter = intersectionResult.Point;
            //        if ((StopPoint == null || StopPoint.X < inter.X))/// && segment.Contains(inter))
            //        {
            //            StopPoint = inter;
            //        }
            //    }
            //}

            //var elapsed0 = DateTime.Now - start0;
            //==> System.Diagnostics.Debug.WriteLine($"Inner: {elapsed0.TotalMilliseconds}");
        }
        Ndl.AddNewPoint(timeEllapsedInMinutes, Math.Min(director.Ndl, 99));
        var elapsed = DateTime.Now - start;
        //=> System.Diagnostics.Debug.WriteLine($"MoveTo:=> {elapsed.TotalMilliseconds} for ALL");
        return director;
    }

    public IEnumerator<BulhmanCompartment> GetEnumerator()
    {
        return ((IEnumerable<BulhmanCompartment>)_compartments).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Reset()
    {
        _tensionByCompartment.Clear();
        _tensionByAmbiantPressureByCompartment.Clear();
        Ndl = new();
        foreach (var compartment in this)
        {
            compartment.Reset();
        }
    }

    /// <summary>
    /// Calculer tout le profil de plongée
    /// </summary>
    /// <param name="diveProfile"></param>
    public void ComputeDiveProfile(RealWorldPoints diveProfile)
    {
        double maxMn = diveProfile.MaxWorldX;
        double sampling = maxMn / 120;
        //Echantillonner toutes N points
        var sampledDiveProfile = diveProfile.Sample(sampling);
        Reset();
        foreach (var compartment in _compartments)
        {
            compartment.SetGradientFactors(GradientFactorsSettings);
        }


        RealWorldPoints compartmentData = new RealWorldPoints();
        RealWorldPoints mValuesData = new RealWorldPoints();

        var startTime = DateTime.Now;

        int countPoints = 0;

        List<Guid> diveProfilePoints = new List<Guid>();
        diveProfile.EnumeratePoints(p => diveProfilePoints.Add(p.Id));

        sampledDiveProfile.EnumeratePoints(p =>
        {
            countPoints++;
            double sec = p.X;
            double prof = -p.Y;
            DirectorCompartment directorCompartment = _moveTo(p.Id, sec, prof);

            if (diveProfilePoints.Contains(p.Id))
            {
                Diver.Computer.AddDiveData(p, directorCompartment);
            }
        });

        var elapsed = DateTime.Now - startTime;
        //==> System.Diagnostics.Debug.WriteLine($"_computeAll: {elapsed.TotalMilliseconds} for COMPUTE ALL {countPoints} points");


    }
}