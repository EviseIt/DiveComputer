using System;

namespace Abo.Buoyancy
{
    public partial class Person:IImmersible
    {
        public Gender Gender { get; set; } = Gender.Femme; // "H" ou "F"
      
        public Person(Gender sexe, decimal tailleCm, decimal poidsKg, int age)
        {
            Gender = sexe;
            SizeCm = tailleCm;
            WeightKg = poidsKg;
            Age = age;
        }

        // --- IMC ---
        public decimal IMC => Convert.ToDecimal((double)WeightKg / Math.Pow(((double)SizeCm) / 100.0, 2));

        // --- Masse grasse estimée (formule Deurenberg, 1991) ---
        public decimal MasseGrassePourcent
        {
            get
            {
                int S = (Gender==Gender.Homme) ? 1 : 0;
                return 1.20m * IMC + 0.23m * Age - 10.8m * S - 5.4m;
            }
        }

        // --- Densité corporelle (kg/L) ---
        public decimal DensiteCorporelle
        {
            get
            {
                decimal fm = MasseGrassePourcent / 100.0m;
                return 1.0m / (fm / 0.900m + (1 - fm) / 1.100m);
            }
        }

        // --- Volume corporel (en litres et en m³) ---
        public decimal VolumeDm3 => WeightKg / DensiteCorporelle;
        public decimal VolumeMetresCube => VolumeDm3 / 1000.0m;

        // --- Surface corporelle (Mosteller) ---
        public decimal SurfaceCorporelle
        {
            get { return Convert.ToDecimal(Math.Sqrt((double)((SizeCm * WeightKg) / 3600.0m))); }
        }

        public void AfficherResultats()
        {
            Console.WriteLine($"Sexe : {Gender}");
            Console.WriteLine($"Taille : {SizeCm} cm");
            Console.WriteLine($"Poids : {WeightKg} kg");
            Console.WriteLine($"Âge : {Age} ans");
            Console.WriteLine($"IMC : {IMC:F1}");
            Console.WriteLine($"Masse grasse estimée : {MasseGrassePourcent:F1} %");
            Console.WriteLine($"Densité corporelle : {DensiteCorporelle:F3} kg/L");
            Console.WriteLine($"Volume : {VolumeDm3:F1} L ({VolumeMetresCube:F3} m³)");
            Console.WriteLine($"Surface corporelle : {SurfaceCorporelle:F2} m²");
        }

        public void Process(int depthMeter, int barLeft, decimal volumeOfAirDm3)
        {
        }

       
    }

    // --- Exemple d'utilisation ---
}
