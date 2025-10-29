using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abo.Buoyancy
{
    public partial class Suit: IImmersible
    {

        private Suit(decimal initialVolumeDm3,decimal initialVolumeOfAir,decimal weightKg)
        {
            _initialVolumeDm3=initialVolumeDm3;
            VolumeDm3 = initialVolumeDm3;
            WeightKg = weightKg;
            _initialVolumeOfAir = initialVolumeOfAir;
        }
        public static decimal KgPerDm3Density = 0.25m;
        public static decimal PercentageAirInNeoprene = 0.8m;
        private readonly decimal _initialVolumeOfAir;
        private readonly decimal _initialVolumeDm3;

        public void Process(int depthMeter, int barLeft, decimal volumeVessie)
        {
            decimal currentVolumeOfAirDm3 =(_initialVolumeOfAir*1)/(1+depthMeter/10.0m);
            VolumeDm3 = _initialVolumeDm3 - (_initialVolumeOfAir -  currentVolumeOfAirDm3);
        }

        public decimal WeightKg { get; private set; }
        public decimal VolumeDm3 { get; private set; }

        public static Suit Build(Person person,int thicknessMm)
        {
            if (person == null) throw new ArgumentNullException(nameof(person));
            decimal initialVolumeDm3=(person.SurfaceCorporelle *10* thicknessMm / 100);
            decimal weightKg = initialVolumeDm3 * KgPerDm3Density;
            decimal initialVolumeOfAir=initialVolumeDm3*PercentageAirInNeoprene;
            return new Suit(initialVolumeDm3, initialVolumeOfAir, weightKg);

        }
    }
}
