namespace Abo.Buoyancy;

public interface IImmersible
{
    void Process(int depthMeter, int barLeft, decimal volumeVessie);
    decimal WeightKg { get; }
    decimal VolumeDm3 { get; }
}