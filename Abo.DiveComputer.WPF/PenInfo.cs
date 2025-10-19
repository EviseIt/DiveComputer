using System.Windows.Media;

namespace RealWorldPlotter;

public class PenInfo(Color color, double width, double[] dashPattern=null)
{
    public Pen ToPen()
    {
        var pen = new Pen(new SolidColorBrush(Color), Width);
        if (dashPattern != null)
        {
            pen.DashStyle = new DashStyle(dashPattern, 0);
        }
        return pen;
    }
    public double Width { get; set; } = width;
    public Color Color { get; set; }= color;

}