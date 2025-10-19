namespace RealWorldPlot.Interfaces.GeometryHelpers;

/// <summary>
/// Droite affine de la forme Y=AX+B
/// </summary>
public class AffineLine
{
    // Propriétés : pente (A) et ordonnée à l'origine (B)
    public double A { get; }
    public double B { get; }

    // Constructeur
    public AffineLine(double a, double b)
    {
        A = a;
        B = b;
    }

    // Méthode statique pour créer une droite affine à partir de deux points
    public static AffineLine FromPoints(RealWorldPoint p1, RealWorldPoint p2)
    {
        if (p1.X == p2.X)
            throw new ArgumentException("Les deux points ont la même abscisse, la pente est infinie.");

        double a = (p2.Y - p1.Y) / (p2.X - p1.X);
        double b = p1.Y - a * p1.X;

        return new AffineLine(a, b);
    }

    // Calcule le point correspondant pour une abscisse x
    public RealWorldPoint GetY(double x)
    {
        return new RealWorldPoint(x, A * x + B);
    }

    /// <summary>
    /// Génère une liste de points équidistants sur la droite affine entre deux abscisses (excluses)
    /// </summary>
    /// <param name="xA">Abscisse du point A (exclue)</param>
    /// <param name="xB">Abscisse du point B (exclue)</param>
    /// <param name="initialStep">Pas initial (sera ajusté si besoin)</param>
    /// <returns>Liste de points entre xA et xB, exclus, répartis uniformément</returns>
    public List<RealWorldPoint> GetIntermediatePoints(double xA, double xB, double initialStep)
    {
        if (xA == xB)
            throw new ArgumentException("xA et xB doivent être différents.");

        if (initialStep <= 0)
            throw new ArgumentException("Le pas (step) doit être strictement positif.");

        // S'assurer que xA < xB
        bool reverse = xB < xA;
        if (reverse)
        {
            (xA, xB) = (xB, xA); // inversion
        }

        double distance = xB - xA;

        // Calcul du nombre de points possibles (exclus de xA et xB)
        int nbPoints = (int)(distance / initialStep) - 1;
        var points = new List<RealWorldPoint>();
        // S'assurer qu'il y a au moins un point intermédiaire
        if (nbPoints >= 1)
        {

            // Recalcul du step pour avoir des points exactement équidistants entre xA et xB
            double step = distance / (nbPoints + 1);

               

            for (int i = 1; i <= nbPoints; i++)
            {
                double x = xA + i * step;
                if (reverse)
                    x = xB - i * step; // si inversion, on repart de xB vers xA

                points.Add(GetY(x));
            }
        }
        return points;
    }
}