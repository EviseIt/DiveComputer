namespace RealWorldPlotter;

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