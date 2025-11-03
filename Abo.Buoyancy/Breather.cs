namespace Abo.Buoyancy;

public partial class Breather : IImmersible
{
    private Breather(string name, decimal weightKg, decimal volumeDm3)
    {
        Name = name;
        WeightKg = weightKg;
        VolumeDm3 = volumeDm3;
    }

    static Breather()
    {
        DetendeurSimple = new Breather("Detendeur simple", 1.5m, 1m);
        DetendeurDouble = new Breather("Détendeur double", 2.5m, 2m);
        Items = [
            DetendeurSimple,
            DetendeurDouble
        ];
    }

    public override string ToString()
    {
        return Name;
    }

    public string Description
    {
        get => $"{Name} {VolumeDm3}L ({WeightKg}kg)";
    }
    public static Breather[] Items { get; }

    public static Breather DetendeurDouble { get; set; }

    public static Breather DetendeurSimple { get; set; }
    public void Process(int depthMeter, int barLeft, decimal volumeVessie)
    {
        
    }
}