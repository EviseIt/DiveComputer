using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace WpfBuoyancy.ViewModels
{
    // Modèle
    public class Arrow
    {
        public System.Windows.Point Start { get; set; }   // (x1,y1)
        public System.Windows.Point End { get; set; }   // (x2,y2)

        public System.Windows.Media.Brush Stroke { get; set; } = System.Windows.Media.Brushes.Black;
        public double Thickness { get; set; } = 2;

        // Paramètres de la tête de flèche
        public double HeadLength { get; set; } = 12;   // en pixels
        public double HeadAngle { get; set; } = 30;   // en degrés
    }

    public class VerticalArrow : Arrow
    {
        public VerticalArrow()
        {

        }
        public void SetY(double y)
        {
            Start = new Point(Start.X, y);
            End = new Point(End.X, y);
        }
        public void SetX(double x)
        {
            Start = new Point(x, Start.Y);
            End = new Point(x, End.Y);
        }

        public void SetLength(decimal length)
        {
            End = new Point(End.X, Start.Y - Convert.ToDouble(length));
        }
    }

    public class WeightArrow: VerticalArrow
        {
        public WeightArrow()
        {
            Stroke=System.Windows.Media.Brushes.Red;
        }

        
    }
    public class LiftArrow : VerticalArrow
    {
        public LiftArrow()
        {
            Stroke = System.Windows.Media.Brushes.Green;
        }
    }
    // ViewModel (exemple)
}
