using System.Windows.Media;
using Abo.DiveComputer.Core;

namespace Abo.DiveComputer.WPF.ViewModels;

public class CompartmentViewModel
{
    public CompartmentViewModel(Compartment compartment, SolidColorBrush solidColorBrush)
    {
        this.Compartment = compartment;
        this.Brush = solidColorBrush;
        this.Name = compartment.Name;
        this.Color = solidColorBrush.Color;
    }

    public Color Color { get; set; }


    public string Name { get; }

    public SolidColorBrush Brush { get; }

    public Compartment Compartment { get; }
}