namespace RealWorldPlot.Interfaces.GeometryHelpers
{
    /// <summary>
    /// Ayant du affines de coeff A et B, trouve l'équation de la droite de gradient fator GF pou GF-H(hih) et GF-L(low)
    /// </summary>
    public class GradientFactor
    {
        private readonly AffineLine _mValues;
        private readonly AffineLine _ambiant;



      

        public GradientFactor(AffineLine mValues,AffineLine ambiant,int gfHPercentage,int gfLowPercentage)
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
        /// </summary>
        /// <param name="xHigh"></param>
        /// <param name="xLow"></param>
        /// <returns></returns>
        public  void SolveForX(double xHigh, double xLow)
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

    public class EuclidianComputer
    {

        public static IntersectionResult FindIntersection(RealWorldPoint P1, RealWorldPoint P2, RealWorldPoint A1, RealWorldPoint A2)
        {
            double x1 = P1.X, y1 = P1.Y;
            double x2 = P2.X, y2 = P2.Y;
            double x3 = A1.X, y3 = A1.Y;
            double x4 = A2.X, y4 = A2.Y;

            double denominator = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);

            if (Math.Abs(denominator) < 1e-10)
            {
                // Droites parallèles ou confondues
                return new IntersectionResult
                {
                    Intersects = false,
                    OnBothSegments = false,
                    Point = null
                };
            }

            double t = ((x1 - x3) * (y3 - y4) - (y1 - y3) * (x3 - x4)) / denominator;
            double u = ((x1 - x3) * (y1 - y2) - (y1 - y3) * (x1 - x2)) / denominator;

            var intersection = new RealWorldPoint(
                x1 + t * (x2 - x1),
                y1 + t * (y2 - y1)
            );

            bool onSegment1 = t >= 0 && t <= 1;
            bool onSegment2 = u >= 0 && u <= 1;

            return new IntersectionResult
            {
                Intersects = true,
                OnBothSegments = onSegment1 && onSegment2,
                Point = intersection
            };
        }
        public static ProjectionResult ProjectPointOnLine(RealWorldPoint P1, RealWorldPoint P2, RealWorldPoint A1)
        {
            double dx = P2.X - P1.X;
            double dy = P2.Y - P1.Y;

            if (Math.Abs(dx) < 1e-10 && Math.Abs(dy) < 1e-10)
            {
                // P1 et P2 confondus : la droite est un point
                double dist = Distance(P1, A1);
                return new ProjectionResult
                {
                    ProjectedPoint = P1,
                    IsOnSegment = true,
                    DistanceToLine = dist
                };
            }

            // Projection scalaire t du vecteur A1P1 sur le vecteur P1P2
            double t = ((A1.X - P1.X) * dx + (A1.Y - P1.Y) * dy) / (dx * dx + dy * dy);

            // Point projeté sur la droite
            RealWorldPoint projected = new RealWorldPoint(P1.X + t * dx, P1.Y + t * dy);

            // Distance entre A1 et la droite (non limitée au segment)
            double distance = Distance(projected, A1);

            // Vérifie si le point projeté est dans le segment [P1, P2]
            bool isOnSegment = t >= 0 && t <= 1;

            return new ProjectionResult
            {
                ProjectedPoint = projected,
                IsOnSegment = isOnSegment,
                DistanceToLine = distance
            };
        }

        // Fonction utilitaire
        private static double Distance(RealWorldPoint a, RealWorldPoint b)
        {
            double dx = a.X - b.X;
            double dy = a.Y - b.Y;
            double dist =Math.Sqrt(dx * dx + dy * dy);
            return dist;
        }

    }
    public class IntersectionResult
    {
        public bool Intersects { get; set; }              // Les droites se croisent
        public bool OnBothSegments { get; set; }          // Le point est sur les deux segments
        public RealWorldPoint? Point { get; set; }                // Coordonnées du point d'intersection (null si aucune)
    }

    public class ProjectionResult
    {
        public RealWorldPoint ProjectedPoint { get; set; }       // Le point projeté
        public bool IsOnSegment { get; set; }            // True si sur le segment P1–P2
        public double DistanceToLine { get; set; }        // Distance de A1 à la droite P1–P2
    }
}
