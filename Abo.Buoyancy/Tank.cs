using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abo.Buoyancy
{
    public partial class Tank:IImmersible
    {
        private readonly decimal _emptyWeightKg;

        private Tank(decimal innerVolumeL,decimal outerVolumeL,decimal weightKg)
        {
            _emptyWeightKg=weightKg;
            WeightKg=_emptyWeightKg;
            VolumeDm3 =outerVolumeL;
            VolumeL=innerVolumeL;
        }
        static Tank()
        {
            TwelveLiters = new Tank(12, 16.9m, 14.5m);
            FifteenLiters = new Tank(15, 23.2m, 17.5m);
            EighteenLiters = new Tank(18, 25.9m, 19.5m);
            Items = [TwelveLiters,
                FifteenLiters,
                EighteenLiters
            ];
        }

        public override string ToString()
        {
            return $"{VolumeL} l";
        }

        public string Description
        {
            get=>$"Acier : {VolumeL}L ({WeightKg}kg)";
        }
        public static Tank[] Items { get;  }

        public static Tank EighteenLiters { get;  }

        public static Tank FifteenLiters { get;  }

        public static Tank TwelveLiters { get;  }
        public void Process(int depthMeter, int barLeft, decimal volumeVessie)
        {
            WeightKg= _emptyWeightKg+barLeft * VolumeL*0.0012m;
        }
        public void Fill(int initialPressureBar)
        {
            InitialPressureBar = initialPressureBar;
        }
        /// <summary>
        /// Poids du bloc en Kg
        /// </summary>
        public decimal WeightKg { get; private set; }
        public decimal VolumeDm3 { get;}
    }
}
