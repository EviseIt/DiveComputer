using RealWorldPlot.Interfaces;

namespace Abo.DiveComputer.Core;

public class Tank
{
    private double _airQuantity;

    public Tank(Diver owner)
    {
        this.Diver = owner;
    }

    public Diver Diver { get;  }

    public void Initialize(double tankCapacityL, double startPressureBar)
    {
        CapacityL = tankCapacityL;
        StartPressureBar= startPressureBar;
        _airQuantity= tankCapacityL*startPressureBar;
    }

    public double StartPressureBar { get; private set; }

    public double CapacityL { get; private set; }

    public void ComputeDiveProfile(RealWorldPoints diveProfile)
    {
        diveProfile.EnumerateByTwoPoints((a, b) =>
        {
            var initPressureBar = _airQuantity / CapacityL;
            Diver.Computer.AddDiveAirData(a, initPressureBar);
            double ellapsedTimeInSeconds = (b.X - a.X);
            double consuptionInL=Diver.SurfaceAirConsuptionLPerS*ellapsedTimeInSeconds;
            _airQuantity -= consuptionInL;
            if(_airQuantity<0){ _airQuantity=0;}
            var pressureBar=_airQuantity/CapacityL;
            Diver.Computer.AddDiveAirData(b, pressureBar);
        });
    }
}