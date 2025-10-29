namespace Abo.Buoyancy;

public class BuoyancyCalculator
{
    public decimal WaterDensityKgPerDm3 = 1;
    public decimal CalculateBuoyancyForceN(IImmersible immersible)
    {
        decimal buoyancyForceN = WaterDensityKgPerDm3 * immersible.VolumeDm3 * 9.81m;
        decimal gravityForce = immersible.WeightKg * 9.81m;
        return gravityForce-buoyancyForceN;
    }
}