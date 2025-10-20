using System.Windows.Media;
using Abo.DiveComputer.Core;
using RealWorldPlot.Interfaces;
using RealWorldPlotter;

namespace Abo.DiveComputer.WPF.ViewModels;

public class CompartmentViewModel
{
    public CompartmentViewModel(BulhmanCompartment compartment, SolidColorBrush solidColorBrush)
    {
        this.Compartment = compartment;
        this.Brush = solidColorBrush;
        this.Name = compartment.Name;
        this.Color = solidColorBrush.Color;
    }

    public Color Color { get; set; }


    public string Name { get; }

    public SolidColorBrush Brush { get; }

    public BulhmanCompartment Compartment { get; }

    public GfGraphViewModel GetGfGraphViewModel()
    {
        GfGraphViewModel gfGraphViewModel = new();
        gfGraphViewModel.MValuesPoints.MinPoint= Compartment.GradientFactorLines.MValues.GetY(1);
        gfGraphViewModel.MValuesPoints.MaxPoint = Compartment.GradientFactorLines.MValues.GetY(Compartment.DiveProfile.MaxAmbiantPressure);

        gfGraphViewModel.AmbiantPoints.MinPoint = N2AmbiantPressure.GetInstance().AffineLine.GetY(1);
        gfGraphViewModel.AmbiantPoints.MaxPoint = N2AmbiantPressure.GetInstance().AffineLine.GetY(Compartment.DiveProfile.MaxAmbiantPressure);

        gfGraphViewModel.GradientFactor.MinPoint= Compartment.GradientFactorLines.AffineLine.GetY(1);
        gfGraphViewModel.GradientFactor.MaxPoint = Compartment.GradientFactorLines.AffineLine.GetY(Compartment.DiveProfile.MaxAmbiantPressure);

        return gfGraphViewModel;
    }
}

public class GfGraphViewModel
{
    public TwoPoints MValuesPoints { get; } = new(new PenInfo(Colors.Crimson, 1.5, new Double[] { 5.0, 5.0, 5.0 }));
    public TwoPoints AmbiantPoints { get; } = new(new PenInfo(Colors.Blue, 1.5, new Double[] { 5.0, 5.0, 5.0 }));
    public TwoPoints GradientFactor { get; } = new(new PenInfo(Colors.Aqua, 1.5, new Double[] { 5.0, 5.0, 5.0 }));
}

public class TwoPoints
{
    public TwoPoints(PenInfo penInfo)
    {
        this.PenInfo = penInfo;
    }
    public static implicit operator RealWorldPoints(TwoPoints twoPoints)
    {
        RealWorldPoints points = new();
        points.AddNewPoint(twoPoints.MinPoint);
        points.AddNewPoint(twoPoints.MaxPoint);
        return points;
    }
    public PenInfo PenInfo { get;  }

    public RealWorldPoint MinPoint { get; set; }
    public RealWorldPoint MaxPoint { get; set; }
}