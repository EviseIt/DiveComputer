namespace Abo.Buoyancy;

public class Program
{
    public static void Main()
    {
        var p = new Person(Gender.Male, 175, 70, 35);
        p.AfficherResultats();
    }
}