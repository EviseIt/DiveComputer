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
            Voyage = new Jacket("Voyage",1, 2.5m, 15);
            LoisirStandard = new Jacket("Loisir standard",2, 4m, 22);
            Technique = new Jacket("Technique",3, 6m, 35);
            Items =
            [
                Voyage,
                LoisirStandard,
                Technique
            ];
        }

        public override string ToString()
        {
            return Name;
        }

        public string Description
        {
            get=>$"{EmptyVolumeDm3}L vide, {_liftMaxVolumeDm3}L max ({_emptyWeight}kg)";
        }
        public static Jacket[] Items { get; }

        public static Jacket Technique { get;  }

        public static Jacket LoisirStandard { get;  }

        public static Jacket Voyage { get;  }


        private readonly decimal _emptyWeight;

        private Jacket(string name,decimal emptyVolumeDm3,decimal emptyWeight,decimal liftMaxVolumeDm3)
        {
            EmptyVolumeDm3 = emptyVolumeDm3;
            _emptyWeight = emptyWeight;
            _liftMaxVolumeDm3 = liftMaxVolumeDm3;
            Name=name;
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
        public string Name { get; private set; }
        public decimal WeightKg { get; private set; }
        public decimal VolumeDm3 { get; private set; }
    }
}
