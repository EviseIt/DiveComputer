using System.Drawing;

namespace RealWorldPlot.Interfaces
{
    public class RealWorldPoint
    {
        public RealWorldPoint(double x, double y)
        {
            X = x;
            Y = y;
        }
        public double X { get; set; }
        public double Y { get; set; }

        protected bool Equals(RealWorldPoint other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((RealWorldPoint)obj);
        }

        public override int GetHashCode()
        {
            //pas de X et Y deux points avec les mêmes coordonnées sont égaux mais n'ont pas la même identité
            return base.GetHashCode();
        }

       
    }
}
