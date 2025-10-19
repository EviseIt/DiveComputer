using System.Windows.Media;

namespace RealWorldPlotter;

public class BrushInfo
{
    public BrushInfo(Color color, double opacity = 1.0)
    {
        Color = color;
        Opacity = opacity;
    }

    public Brush ToBrush()
    {
        return new SolidColorBrush(Color) { Opacity = Opacity };
    }

    public Color Color { get; set; }
    public double Opacity { get; set; } = 1.0;
}