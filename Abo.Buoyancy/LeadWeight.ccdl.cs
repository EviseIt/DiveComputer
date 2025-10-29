namespace Abo.Buoyancy
{
    public partial class LeadWeight
    {
        private decimal _weightKg;
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
    }
}

