namespace Abo.DiveComputer.Core;

/// <summary>
/// Compratiment value with tension and NDL
/// </summary>
public class BulhmanCompartmentValue
{
    public BulhmanCompartmentValue(double tension, double ndl, double gfMaxN2Tissue)
    {
        this.TensionN2 = tension;
        this.Ndl = ndl;
        this.GfMaxN2Tissue = gfMaxN2Tissue;
    }
    /// <summary>
    /// Tension maximale N2 tissulaire pour le gradient factor en bar
    /// </summary>
    public double GfMaxN2Tissue { get; }

    public double Ndl { get; }
    /// <summary>
    /// Tension en N2 des tissus en bar
    /// </summary>
    public double TensionN2 { get; }
}