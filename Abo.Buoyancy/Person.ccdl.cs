namespace Abo.Buoyancy
{
    public partial class Person
    {
        private decimal _weightKg;
        private decimal _sizeCm;
        private int _age;
        ///<summary>
        ///Poids du lest en Kg
        ///WeightKg must be >=0 and <=200.4
        ///</summary>
        public decimal WeightKg
        {
            get
            {
                return _weightKg;
            }
            set
            {
                if (value < 0m || value > 200.4m)
                { throw new Exception("WeightKg must be >=0 and <=200.4"); }
                if (_weightKg != value)
                {
                    _weightKg = value;
                }
            }
        }
        ///<summary>
        ///Taille en cm
        ///SizeCm must be >=100 and <=240
        ///</summary>
        public decimal SizeCm
        {
            get
            {
                return _sizeCm;
            }
            set
            {
                if (value < 100m || value > 240m)
                { throw new Exception("SizeCm must be >=100 and <=240"); }
                if (_sizeCm != value)
                {
                    _sizeCm = value;
                }
            }
        }
        ///<summary>
        ///Age en années
        ///Age must be >=10 and <=120
        ///</summary>
        public int Age
        {
            get
            {
                return _age;
            }
            set
            {
                if (value < 10 || value > 120)
                { throw new Exception("Age must be >=10 and <=120"); }
                if (_age != value)
                {
                    _age = value;
                }
            }
        }
    }
}