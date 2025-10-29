using Abo.Buoyancy;

namespace BuyancyConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Diver diver = Diver.New(new Person(Gender.Male, 174,92, 52), 12, Jacket.Voyage, Tank.FifteenLiters, 200, Breather.DetendeurDouble, Fins.ScubaproSeawingNova, new LeadWeight(){WeightKg = 2});
            diver.Process(0, 200,1.75m);
            Console.WriteLine(diver.WeightKg);
            Console.WriteLine(diver.VolumeDm3);

            Console.WriteLine(diver.WeightKg-diver.VolumeDm3);
        }
    }
}
