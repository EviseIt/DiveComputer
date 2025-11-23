namespace Abo.Buoyancy;


public class Water
{
    public string Name { get; }


    private Water(string name,decimal density)
    {
        Name = name;
        Density = density;
    }

    public decimal Density { get; set; }

    static Water()
    {
        StillWater = new Water("Eau douce",1.0m);
        SeaWater = new Water("Eau salée",1.029m);
        Items = [
            StillWater,
            SeaWater
        ];
    }

    public override string ToString()
    {
        return Name;
    }

    public static Water[] Items { get; }
    public static Water StillWater { get; }

    public static Water SeaWater { get; }
}
public class Gender
{
    public string Name { get; }


    private Gender(string name)
    {
        Name = name;
    }
    static Gender()
    {
        Homme = new Gender("Homme");
        Femme = new Gender("Femme");
        Items = [
            Homme,
            Femme
        ];
    }

    public override string ToString()
    {
        return Name;
    }

    public static Gender[] Items { get; }
    public static Gender Femme { get;  }

    public static Gender Homme { get;  }
}