namespace RealWorldPlot.Interfaces.GeometryHelpers
{
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
}
