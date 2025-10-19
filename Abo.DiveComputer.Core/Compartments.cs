using System.Collections;
using RealWorldPlot.Interfaces;

namespace Abo.DiveComputer.Core;

public class Compartments : IEnumerable<Compartment>
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
            if (Compartments.DEBUG)
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

    public void SetGradientFactors(int low, int high)
    {
        foreach (var compartment in _compartments)
        {
            compartment.SetGradientFactors(low, high);
        }
    }


    private readonly Compartment[] _compartments;
    public Compartments(Diver diver)
    {
        this.Diver = diver;
        _compartments = new Compartment[]
        {

            new Compartment(4,1.9082,1.2599),
            new Compartment(5,1.7928,1.1696),
            new Compartment(8,1.5352,1),
            new Compartment(12.5,1.3847,0.8618),
            new Compartment(18.5,1.278,0.7562),
            new Compartment(27,1.2306,0.62),
            new Compartment(38.3,1.1857,0.5043),
            new Compartment(54.3,1.1504,0.441),
            new Compartment(77,1.1223,0.375),
            new Compartment(109,1.0999,0.35),
            new Compartment(146,1.0844,0.3295),
            new Compartment(187,1.0731,0.3065),
            new Compartment(239,1.0635,0.2835),
            new Compartment(305,1.0552,0.285),
            new Compartment(390,1.0478,0.261),
            new Compartment(498,1.0414,0.248),
            new Compartment(635,1.0359,0.2327),


        };



    }

    public Diver Diver { get; }

    /// <summary>
    /// Tensions par compartiment
    /// </summary>
    private readonly Dictionary<Compartment, RealWorldPoints> _tensionByCompartment = new();
    /// <summary>
    /// Donnnées de la courbe de plongée sur le graph des M-values pour un compartiment donné.
    /// </summary>
    private readonly Dictionary<Compartment, RealWorldPoints> _mValueDataByCompartment = new();
    public RealWorldPoints Ndl = new();
    public static bool DEBUG = false;
    public Compartment this[int index] => _compartments[index];

    public RealWorldPoints GetTensions(Compartment compartment)
    {
        return _tensionByCompartment[compartment];
    }
    /// <summary>
    /// Obtient les données de la courbe à de plongée sur le graph des M-values pour un compartiment donné.
    /// </summary>
    /// <param name="compartment"></param>
    /// <returns></returns>
    public RealWorldPoints GetMValuesData(Compartment compartment)
    {
        return _mValueDataByCompartment[compartment];
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
            CompartmentValue compartmentValue = compartment.MoveTo(diveProfilePointId, 60 * timeEllapsedInMinutes, depth);
            double n2Tension = compartmentValue.TensionN2;
            double ambPressure = (Math.Abs(depth) / 10.0) + Compartments.PSurfaceBar;

            DateTime start0 = DateTime.Now;

            if (!_tensionByCompartment.TryGetValue(compartment, out RealWorldPoints? compartmentData))
            {
                compartmentData = new RealWorldPoints();
                _tensionByCompartment.Add(compartment, compartmentData);

            }
            compartmentData.AddNewPoint(timeEllapsedInMinutes, n2Tension);

            director.ComputeDirector(compartment, n2Tension, compartmentValue.Ndl);

            if (!_mValueDataByCompartment.TryGetValue(compartment, out RealWorldPoints? mValueData))
            {
                mValueData = new RealWorldPoints();
                _mValueDataByCompartment.Add(compartment, mValueData);
            }
            mValueData.AddNewPoint(ambPressure, n2Tension);
            //var elapsed0 = DateTime.Now - start0;
            //==> System.Diagnostics.Debug.WriteLine($"Inner: {elapsed0.TotalMilliseconds}");
        }
        Ndl.AddNewPoint(timeEllapsedInMinutes, Math.Min(director.Ndl, 99));
        var elapsed = DateTime.Now - start;
        //=> System.Diagnostics.Debug.WriteLine($"MoveTo:=> {elapsed.TotalMilliseconds} for ALL");
        return director;
    }

    public IEnumerator<Compartment> GetEnumerator()
    {
        return ((IEnumerable<Compartment>)_compartments).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Reset()
    {
        _tensionByCompartment.Clear();
        _mValueDataByCompartment.Clear();
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