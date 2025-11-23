using System.Windows;
using System.Windows.Media;

namespace WpfBuoyancy.Controls;

public class ArrowSpec : Freezable
{
    public double DownLength { get => (double)GetValue(DownLengthProperty); set => SetValue(DownLengthProperty, value); }
    public static readonly DependencyProperty DownLengthProperty =
        DependencyProperty.Register(nameof(DownLength), typeof(double), typeof(ArrowSpec), new FrameworkPropertyMetadata(40.0));

    public double UpLength { get => (double)GetValue(UpLengthProperty); set => SetValue(UpLengthProperty, value); }
    public static readonly DependencyProperty UpLengthProperty =
        DependencyProperty.Register(nameof(UpLength), typeof(double), typeof(ArrowSpec), new FrameworkPropertyMetadata(40.0));


    public Point HookPoint { get => (Point)GetValue(HookPointProperty); set => SetValue(HookPointProperty, value); }
    public static readonly DependencyProperty HookPointProperty =
        DependencyProperty.Register(nameof(HookPoint), typeof(Point), typeof(ArrowSpec), new FrameworkPropertyMetadata(new Point(0.5, 0.5)));

    public bool HookIsNormalized { get => (bool)GetValue(HookIsNormalizedProperty); set => SetValue(HookIsNormalizedProperty, value); }
    public static readonly DependencyProperty HookIsNormalizedProperty =
        DependencyProperty.Register(nameof(HookIsNormalized), typeof(bool), typeof(ArrowSpec), new FrameworkPropertyMetadata(true));

    public Brush Brush { get => (Brush)GetValue(BrushProperty); set => SetValue(BrushProperty, value); }
    public static readonly DependencyProperty BrushProperty =
        DependencyProperty.Register(nameof(Brush), typeof(Brush), typeof(ArrowSpec), new FrameworkPropertyMetadata(null));

    public Brush UpBrush { get => (Brush)GetValue(UpBrushProperty); set => SetValue(UpBrushProperty, value); }
    public static readonly DependencyProperty UpBrushProperty =
        DependencyProperty.Register(nameof(UpBrush), typeof(Brush), typeof(ArrowSpec), new FrameworkPropertyMetadata(null));

    public Brush DownBrush { get => (Brush)GetValue(DownBrushProperty); set => SetValue(DownBrushProperty, value); }
    public static readonly DependencyProperty DownBrushProperty =
        DependencyProperty.Register(nameof(DownBrush), typeof(Brush), typeof(ArrowSpec), new FrameworkPropertyMetadata(null));

    public double StrokeThickness { get => (double)GetValue(StrokeThicknessProperty); set => SetValue(StrokeThicknessProperty, value); }
    public static readonly DependencyProperty StrokeThicknessProperty =
        DependencyProperty.Register(nameof(StrokeThickness), typeof(double), typeof(ArrowSpec), new FrameworkPropertyMetadata(double.NaN));

    public double ArrowHeadSize { get => (double)GetValue(ArrowHeadSizeProperty); set => SetValue(ArrowHeadSizeProperty, value); }
    public static readonly DependencyProperty ArrowHeadSizeProperty =
        DependencyProperty.Register(nameof(ArrowHeadSize), typeof(double), typeof(ArrowSpec), new FrameworkPropertyMetadata(double.NaN));

    protected override Freezable CreateInstanceCore() => new ArrowSpec();
}