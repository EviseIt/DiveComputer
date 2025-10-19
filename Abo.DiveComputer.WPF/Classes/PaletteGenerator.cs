using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abo.DiveComputer.WPF.Classes
{
    using System;
    using System.Linq;
    using System.Windows.Media;

    public static class PaletteGenerator
    {


        /// <summary>
        /// Génère N brosses SolidColorBrush avec des teintes HSL uniformément espacées.
        /// s et l sont compris entre 0..1 (saturation et luminosité).
        /// </summary>
        public static SolidColorBrush[] GenerateDistinct(int n = 17, double s = 0.65, double l = 0.55, double hueStart = 10.0)
        {
            if (n <= 0)
                return Array.Empty<SolidColorBrush>();
            double step = 360.0 / n;

            return Enumerable.Range(0, n)
                .Select(i =>
                {
                    double h = (hueStart + i * step) % 360.0;
                    var c = HslToColor(h, s, l);
                    return new SolidColorBrush(c);
                })
                .ToArray();
        }

        // Conversion HSL -> Color (RGB 0..255)
        private static Color HslToColor(double h, double s, double l)
        {
            h = ((h % 360.0) + 360.0) % 360.0;
            double c = (1 - Math.Abs(2 * l - 1)) * s;
            double hp = h / 60.0;
            double x = c * (1 - Math.Abs(hp % 2 - 1));

            double r1 = 0, g1 = 0, b1 = 0;
            if (0 <= hp && hp < 1)
                (r1, g1, b1) = (c, x, 0);
            else if (1 <= hp && hp < 2)
                (r1, g1, b1) = (x, c, 0);
            else if (2 <= hp && hp < 3)
                (r1, g1, b1) = (0, c, x);
            else if (3 <= hp && hp < 4)
                (r1, g1, b1) = (0, x, c);
            else if (4 <= hp && hp < 5)
                (r1, g1, b1) = (x, 0, c);
            else if (5 <= hp && hp < 6)
                (r1, g1, b1) = (c, 0, x);

            double m = l - c / 2.0;
            byte R = (byte)Math.Round((r1 + m) * 255.0);
            byte G = (byte)Math.Round((g1 + m) * 255.0);
            byte B = (byte)Math.Round((b1 + m) * 255.0);

            return Color.FromRgb(R, G, B);
        }
    }

}
