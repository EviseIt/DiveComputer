namespace Abo.Buoyancy;

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