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
        public System.Windows.Point StartPoint { get; set; }   // (x1,y1)
        public System.Windows.Point EndPoint { get; set; }   // (x2,y2)


        public System.Windows.Point Start
        {
            get => new Point(StartPoint.X + OffsetX+ImageOffsetX,StartPoint.Y+OffsetY+ImageOffsetY);
        }   // (x1,y1)
        public double ImageOffsetX { get; set; } = 0;
        public double ImageOffsetY { get; set; } = 0;
        public double OffsetX { get; set; } = 0;
        public double OffsetY { get; set; } = 0;

        public System.Windows.Point End
        {
            get => new Point(EndPoint.X + OffsetX+ImageOffsetX, EndPoint.Y + OffsetY+ImageOffsetY);
        }   // (x2,y2)

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
            StartPoint = new Point(StartPoint.X, y);
            EndPoint = new Point(EndPoint.X, y);
        }
        public void SetX(double x)
        {
            StartPoint = new Point(x, StartPoint.Y);
            EndPoint = new Point(x, EndPoint.Y);
        }

        public void SetLength(decimal length)
        {
            EndPoint = new Point(EndPoint.X, StartPoint.Y - Convert.ToDouble(length));
        }
    }

    public class WeightArrow : VerticalArrow
    {
        public WeightArrow()
        {
            Stroke = System.Windows.Media.Brushes.Red;
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
