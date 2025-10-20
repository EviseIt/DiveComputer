namespace RealWorldPlot.Interfaces.GeometryHelpers;

/// <summary>
/// Ayant du affines de coeff A et B, trouve l'équation de la droite de gradient fator GF pou GF-H(hih) et GF-L(low)
/// </summary>
public class GradientFactorLines
{
    private readonly AffineLine _mValues;//MValues
    private readonly AffineLine _ambiant;//Ambiant pressure





    public GradientFactorLines(AffineLine mValues,AffineLine ambiant,int gfHPercentage,int gfLowPercentage)
    {
        _mValues = mValues;
        _ambiant = ambiant;
        GFH = 100-gfHPercentage;
        GFL = 100-gfLowPercentage;

    }
    public int GFL { get; }
    public int GFH { get;  }
    /// <summary>
    /// ATENTION xHigh  < xLow  (Hiht Low au sens de la profondeur)
    /// Résout l'équation de la droite de gradient factor par rapport aux Mvalues et à la droite des pressions ambiantes pour avoir la droite des GF
    /// </summary>
    /// <param name="xHigh"></param>
    /// <param name="xLow"></param>
    /// <returns></returns>
    public void SolveForX(double xHigh, double xLow)
    {
        //DELTA X=distance entre les deux points X2 et X1 tels que Y2=Y1=A2X2+B2=A1X1+B1
        double x1 = xLow;
        double deltaLow=((_mValues.A-_ambiant.A)*x1+_mValues.B-_ambiant.B)/_ambiant.A;
        double xResultLow= x1 + deltaLow * ((double)GFL/ 100.00);
        double yResultLow = _mValues.GetY(xLow).Y;
        //=>Point haut de la droite GF xresult,yresult

        //DELTA Y=distance entre les deux points Y2 et Y1 tels que Y2=A2X+B2 et Y=A1X+B1 et X=cte
        double deltaHigh = (_ambiant.A - _mValues.A) * xHigh + _ambiant.B - _mValues.B;
        double yResultHigh = _mValues.GetY(xHigh).Y+ deltaHigh * ((double)GFH / 100.00);
        double xResultHigh = xHigh;

        this.High = new RealWorldPoint(xResultHigh, yResultHigh);
        this.Low = new RealWorldPoint(xResultLow, yResultLow);
        this.Points = new RealWorldPoints();
        this.Points.AddNewPoint(this.High);
        this.Points.AddNewPoint(this.Low);
        this.AffineLine = AffineLine.FromPoints(this.Low, this.High);
    }

    public AffineLine AffineLine { get; private set; }

    public RealWorldPoints Points { get; private set; }
    public RealWorldPoint Low { get; private set; }

    public RealWorldPoint High { get; private set; }
}