namespace WpfBuoyancy.ViewModels;

public class DiagramViewModel
{
    public System.Collections.ObjectModel.ObservableCollection<Arrow> Arrows { get; } =
        new System.Collections.ObjectModel.ObservableCollection<Arrow>
        {
            new Arrow { Start = new(20,20), End = new(200,80), Stroke = System.Windows.Media.Brushes.CornflowerBlue, Thickness = 2 },
            new Arrow { Start = new(50,150), End = new(260,150), Stroke = System.Windows.Media.Brushes.IndianRed, HeadLength=16, HeadAngle=25 }
        };
}