namespace Abo.DiveComputer.Core;

public class HaldaneCompartment
{
    private double _n2Pressure;
    private readonly double _halfLife;

    public HaldaneCompartment(double halfLife, double aBulhmanCoeff, double bBulhmanCoeff)
    {
        this._halfLife = halfLife;
        _n2Pressure = _computeBreathedN2Pressure(0);
        this.Name = $"{halfLife} mn";
        ABulhmanCoeff = aBulhmanCoeff;
        BBulhmanCoeff = bBulhmanCoeff;
    }

    public double BBulhmanCoeff { get; }

    public double ABulhmanCoeff { get; }

    protected bool Equals(HaldaneCompartment other)
    {
        return _halfLife == other._halfLife;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != this.GetType())
            return false;
        return Equals((BulhmanCompartment)obj);
    }

    public override int GetHashCode()
    {
        return _halfLife.GetHashCode();
    }

    public string Name { get; }
    public double CurrentN2Pressure => _n2Pressure;
    private double _computeAmbiantPressure(double depthInMeter) => 1.0 + depthInMeter / 10.0;
    private double _computeBreathedN2Pressure(double profondeurMetres) => _computeAmbiantPressure(profondeurMetres) * 0.79;

    public double MoveTo(double ellapsedTimeInSeconds, double profondeur)
    {
        double n2BreathedPressure = _computeBreathedN2Pressure(profondeur);
        double k = (double)(Math.Log(2)) / _halfLife;
        double exponentialFactor = (double)(-k * (double)(ellapsedTimeInSeconds / 60.0));
        _n2Pressure += (n2BreathedPressure - _n2Pressure) * (1 - (double)(Math.Exp(exponentialFactor)));
        return (double)_n2Pressure;
    }


    public void Reset()
    {
        _n2Pressure = _computeBreathedN2Pressure(0);
    }
}