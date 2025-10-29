namespace Abo.Buoyancy;

public partial class Fins : IImmersible
{
    private Fins(string name,decimal weightKg,decimal volumeDm3)
    {
        Name = name;
        WeightKg = weightKg;
        VolumeDm3 = volumeDm3;
    }

    static Fins()
    {
        MaresAvantiQuattroPlus = new Fins("Mares Avanti Quattro +", 2.2m, 4m);
        ScubaproSeawingNova = new Fins("Scubapro Seawing Nova", 2, 3.5m);
        AqualungStratos3 = new Fins("Aqualung Stratos 3", 1.8m, 3.2m);
    }

    public static Fins AqualungStratos3 { get;  }

    public static Fins ScubaproSeawingNova { get;  }

    public static Fins MaresAvantiQuattroPlus { get;  }
    public void Process(int depthMeter, int barLeft, decimal volumeVessie)
    {
       
    }
}