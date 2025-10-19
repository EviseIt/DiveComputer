using RealWorldPlot.Interfaces;

namespace Abo.DiveComputer.Core;

public class Diver
{
    private bool _initialized;
    public Compartments Compartments { get; }
    public Tank Tank { get; }
    public Computer Computer { get; }
    public Diver()
    {
        Tank= new Tank(this);
        Computer= new Computer(this);
        Compartments=new Compartments(this);
    }
    public void Initialize(double surfaceAirConsuptionLPerMn,double tankCapacityL, double startPressureBar)
    {
        _initialized = true;
        Tank.Initialize(tankCapacityL, startPressureBar);
        SurfaceAirConsuptionLPerS = surfaceAirConsuptionLPerMn / 60;
    }

    public double SurfaceAirConsuptionLPerS { get; private set; }

    public void ComputeDiveProfile(RealWorldPoints diveProfile)
    {
        if(!_initialized)
            throw new InvalidOperationException("Diver not initialized");
        Compartments.ComputeDiveProfile(diveProfile);
        Tank.ComputeDiveProfile(diveProfile);
    }
}