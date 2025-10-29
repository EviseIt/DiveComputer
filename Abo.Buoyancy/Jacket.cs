using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abo.Buoyancy
{
    public partial class Jacket: IImmersible
    {

        static Jacket()
        {
            Voyage = new Jacket(1, 2.5m, 15);
            LoisirStandard = new Jacket(2, 4m, 22);
            Technique = new Jacket(3, 6m, 35);
        }

        public static Jacket Technique { get;  }

        public static Jacket LoisirStandard { get;  }

        public static Jacket Voyage { get;  }


        private readonly decimal _emptyWeight;

        private Jacket(decimal emptyVolumeDm3,decimal emptyWeight,decimal liftMaxVolumeDm3)
        {
            EmptyVolumeDm3 = emptyVolumeDm3;
            _emptyWeight = emptyWeight;
            _liftMaxVolumeDm3 = liftMaxVolumeDm3;
        }
        private decimal _volumeOfAirDm3;
        private readonly decimal _liftMaxVolumeDm3;

        ///<summary>
        ///Volume de gonflage
        ///VolumeOfAirDm3 must be >=0 and <=30
        ///</summary>
        public decimal VolumeOfAirDm3
        {
            get
            {
                return _volumeOfAirDm3;
            }
            set
            {
                if (value < 0m || value > _liftMaxVolumeDm3)
                { throw new Exception("VolumeOfAirDm3 must be >=0 and <=30"); }
                if (_volumeOfAirDm3 != value)
                {
                    _volumeOfAirDm3 = value;
                }
            }
        }
        public void Process(int depthMeter, int barLeft, decimal volumeVessie)
        {
            VolumeOfAirDm3=volumeVessie;
            WeightKg = _emptyWeight;
            VolumeDm3=_emptyVolumeDm3+ VolumeOfAirDm3;
        }

        public decimal WeightKg { get; private set; }
        public decimal VolumeDm3 { get; private set; }
    }
}
