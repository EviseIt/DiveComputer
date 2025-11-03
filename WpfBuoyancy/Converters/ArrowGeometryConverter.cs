using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfBuoyancy;

public class ArrowGeometryConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        double x1 = (double)values[0];
        double y1 = (double)values[1];
        double x2 = (double)values[2];
        double y2 = (double)values[3];
        double headLen = values.Length > 4 ? System.Convert.ToDouble(values[4]) : 12.0;
        double headDeg = values.Length > 5 ? System.Convert.ToDouble(values[5]) : 30.0;

        double angle = Math.Atan2(y2 - y1, x2 - x1);
        double a = headDeg * Math.PI / 180.0;

        // Points de la tête de flèche
        Point left = new(
            x2 - headLen * Math.Cos(angle - a),
            y2 - headLen * Math.Sin(angle - a));

        Point right = new(
            x2 - headLen * Math.Cos(angle + a),
            y2 - headLen * Math.Sin(angle + a));

        // Construire la géométrie : trait principal + 2 segments pour la tête
        var g = new StreamGeometry();
        using (var ctx = g.Open())
        {
            // Corps
            ctx.BeginFigure(new Point(x1, y1), isFilled: false, isClosed: false);
            ctx.LineTo(new Point(x2, y2), isStroked: true, isSmoothJoin: false);

            // Aile gauche
            ctx.BeginFigure(new Point(x2, y2), isFilled: false, isClosed: false);
            ctx.LineTo(left, isStroked: true, isSmoothJoin: false);

            // Aile droite
            ctx.BeginFigure(new Point(x2, y2), isFilled: false, isClosed: false);
            ctx.LineTo(right, isStroked: true, isSmoothJoin: false);
        }

        g.Freeze();
        return g;
    }


    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}