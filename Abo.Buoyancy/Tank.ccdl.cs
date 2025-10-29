namespace Abo.Buoyancy
{
    public partial class Tank
    {
        private decimal _volumeL;
        private decimal _initialPressureBar;
        ///<summary>
        ///Volume du bloc en litres
        ///VolumeL must be >=5 and <=20
        ///</summary>
        public decimal VolumeL
        {
            get
            {
                return _volumeL;
            }
            set
            {
                if (value < 5m || value > 20m)
                { throw new Exception("VolumeL must be >=5 and <=20"); }
                if (_volumeL != value)
                {
                    _volumeL = value;
                }
            }
        }
        ///<summary>
        ///Pression initiale en bars
        ///InitialPressureBar must be >=1 and <=300
        ///</summary>
        public decimal InitialPressureBar
        {
            get
            {
                return _initialPressureBar;
            }
            set
            {
                if (value < 1m || value > 300m)
                { throw new Exception("InitialPressureBar must be >=1 and <=300"); }
                if (_initialPressureBar != value)
                {
                    _initialPressureBar = value;
                }
            }
        }
    }
}

