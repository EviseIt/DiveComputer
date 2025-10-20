using RealWorldPlot.Interfaces;

namespace Abo.DiveComputer.Core;

public class Diver
{
    private bool _initialized;
    public BulhmanCompartments Compartments { get; }
    public Tank Tank { get; }
    public Computer Computer { get; }
    public Diver()
    {
        Tank= new Tank(this);
        Computer= new Computer(this);
        Compartments=new BulhmanCompartments(this);
    }
    public void Initialize(double surfaceAirConsuptionLPerMn,double tankCapacityL, double startPressureBar)
    {
        _initialized = true;
        Tank.Initialize(tankCapacityL, startPressureBar);
        SurfaceAirConsuptionLPerS = surfaceAirConsuptionLPerMn / 60;
    }

    public double SurfaceAirConsuptionLPerS { get; private set; }

    public void ComputeDiveProfile(DiveProfile diveProfile)
    {
        if(!_initialized)
            throw new InvalidOperationException("Diver not initialized");
        Compartments.ComputeDiveProfile(diveProfile);
        Tank.ComputeDiveProfile(diveProfile);
    }
}