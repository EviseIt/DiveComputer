namespace Abo.Buoyancy;

public class Program
{
    public static void Main()
    {
        var p = new Person(Gender.Homme, 175, 70, 35);
        p.AfficherResultats();
    }
}